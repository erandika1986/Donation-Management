using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.Helpers;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Campaign;

namespace ViharaFund.Infrastructure.Services
{
    public class CampaignService(
        TenantDbContext tenantDbContext,
        IDateTime dateTime,
        ICurrentUserService currentUserService) : ICampaignService
    {
        public async Task<ResultDto> CreateCampaignAsync(CampaignDTO campaignDto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(campaignDto.Name))
                return ResultDto.Failure(new[] { "Campaign name is required." });

            if (campaignDto.TargetAmount < 0)
                return ResultDto.Failure(new[] { "Target amount must be positive." });

            try
            {
                // Map DTO to Entity
                var campaign = new Campaign
                {
                    Name = campaignDto.Name,
                    Description = campaignDto.Description,
                    CampaignCategoryId = campaignDto.CampaignCategoryId,
                    Visibility = campaignDto.Visibility,
                    Status = campaignDto.Status,
                    StartDate = campaignDto.StartDate.Value,
                    HasEndDate = campaignDto.HasEndDate,
                    EndDate = campaignDto.HasEndDate ? campaignDto.EndDate : null,
                    CurrencyTypeId = campaignDto.CurrencyTypeId,
                    CompaignImageUrl = campaignDto.CompaignImageUrl,
                    TargetAmount = campaignDto.TargetAmount,
                    CreatedByUserId = currentUserService.UserId,
                    CreatedDate = dateTime.Now,
                    UpdatedByUserId = currentUserService.UserId,
                    UpdatedDate = dateTime.Now,
                    IsActive = true
                };

                // Add to DbContext
                tenantDbContext.Campaigns.Add(campaign);
                await tenantDbContext.SaveChangesAsync();

                return ResultDto.Success("Campaign created successfully.", campaign.Id);
            }
            catch (Exception ex)
            {
                return ResultDto.Failure(new[] { "Failed to create campaign.", ex.Message });
            }
        }

        public async Task<ResultDto> DeleteCampaignAsync(int id)
        {
            try
            {
                var campaign = await tenantDbContext.Campaigns.FindAsync(id);
                if (campaign == null)
                    return ResultDto.Failure(new[] { "Campaign not found." });

                if (!campaign.IsActive)
                    return ResultDto.Failure(new[] { "Campaign is already deleted." });

                campaign.IsActive = false;
                campaign.UpdatedByUserId = currentUserService.UserId;
                campaign.UpdatedDate = dateTime.Now;

                await tenantDbContext.SaveChangesAsync();
                return ResultDto.Success("Campaign deleted successfully.", campaign.Id);
            }
            catch (Exception ex)
            {
                return ResultDto.Failure(new[] { "Failed to delete campaign.", ex.Message });
            }
        }

        public async Task<List<DropDownDTO>> GetActiveCampaignsAsync()
        {
            var campaigns = await tenantDbContext.Campaigns
                .Where(c => c.IsActive && c.Status == CampaignStatus.Active)
                .OrderBy(c => c.Name)
                .Select(c => new DropDownDTO
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return campaigns;
        }

        public async Task<List<CampaignDTO>> GetAllCampaignsAsync(CampaignFilterDTO campaignFilter)
        {
            var query = tenantDbContext.Campaigns.AsQueryable();

            // Apply StatusId filter if not zero
            query = ApplyCampaignFilters(query, campaignFilter);

            // Pagination logic
            int page = campaignFilter.CurrentPage > 0 ? campaignFilter.CurrentPage : 1;
            int pageSize = campaignFilter.PageSize > 0 ? campaignFilter.PageSize : 10;
            int skip = (page - 1) * pageSize;

            var campaigns = await query
                .OrderByDescending(c => c.CreatedDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new CampaignDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CurrencyTypeId = c.CurrencyTypeId,
                    CampaignCategoryId = c.CampaignCategoryId,
                    CompaignImageUrl = c.CompaignImageUrl,
                    Visibility = c.Visibility,
                    Status = c.Status
                    // Add other properties as needed
                })
                .ToListAsync();

            return campaigns;
        }

        public async Task<CampaignDTO> GetCampaignByIdAsync(int id)
        {
            if (id == 0)
            {
                var defaultCurrencyTypeRecord = await tenantDbContext.AppSettings.FirstOrDefaultAsync(x => x.Name == CompanySettingConstants.DefaultCurrencyId);
                int defaultCurrencyTypeId = Convert.ToInt32(defaultCurrencyTypeRecord.Value);
                return new CampaignDTO()
                {
                    Id = 0,
                    Name = string.Empty,
                    Description = string.Empty,
                    CurrencyTypeId = tenantDbContext.CurrencyTypes.FirstOrDefault(x => x.Id == defaultCurrencyTypeId).Id,
                    CampaignCategoryId = tenantDbContext.CampaignCategories.FirstOrDefault().Id,
                    CompaignImageUrl = string.Empty,
                    Visibility = CampaignVisibility.Public,
                    Status = CampaignStatus.Active,
                    StartDate = DateTime.UtcNow,
                    HasEndDate = true,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    TargetAmount = 0,
                };
            }
            var campaign = await tenantDbContext.Campaigns
                .Include(c => c.CampaignCategory)
                .Include(c => c.CurrencyType)
                .Include(c => c.Donations)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (campaign == null)
                return null;

            return new CampaignDTO
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,
                CurrencyTypeId = campaign.CurrencyTypeId,
                CampaignCategoryId = campaign.CampaignCategoryId,
                Visibility = campaign.Visibility,
                Status = campaign.Status,
                StartDate = campaign.StartDate,
                HasEndDate = campaign.HasEndDate,
                EndDate = campaign.HasEndDate ? campaign.EndDate : null,
                CompaignImageUrl = campaign.CompaignImageUrl,
                TargetAmount = campaign.TargetAmount
            };
        }

        public async Task<CampaignMasterDataDTO> GetCampaignMasterDataAsync()
        {
            var masterData = new CampaignMasterDataDTO();
            masterData.CampaignStatuses.Add(new DropDownDTO() { Id = 0, Name = "All Campaigns" });
            masterData.CampaignCategories.Add(new DropDownDTO() { Id = 0, Name = "All Categories" });

            // Step 1: Get CurrencyTypes from Db
            var currencyTypes = await tenantDbContext.CurrencyTypes
                .OrderBy(x => x.Name)
                .Select(ct => new DropDownDTO
                {
                    Id = ct.Id,
                    Name = ct.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
            masterData.CurrencyTypes.AddRange(currencyTypes);

            // Step 2: Get CampaignCategories from Db
            var campaignCategories = await tenantDbContext.CampaignCategories
                .Select(cc => new DropDownDTO
                {
                    Id = cc.Id,
                    Name = cc.Name
                })
                .ToListAsync();
            masterData.CampaignCategories.AddRange(campaignCategories);

            // Step 3: Get CampaignVisibilities from Enum
            var campaignVisibilities = Enum.GetValues(typeof(CampaignVisibility))
                .Cast<CampaignVisibility>()
                .Select(v => new DropDownDTO
                {
                    Id = (int)v,
                    Name = EnumHelper.GetEnumDescription(v)
                })
                .ToList();
            masterData.CampaignVisibilities.AddRange(campaignVisibilities);

            // Step 4: Get CampaignStatuses from Enum
            var campaignStatuses = Enum.GetValues(typeof(CampaignStatus))
                .Cast<CampaignStatus>()
                .Select(s => new DropDownDTO
                {
                    Id = (int)s,
                    Name = EnumHelper.GetEnumDescription(s)
                })
                .OrderBy(x => x.Name)
                .ToList();
            masterData.CampaignStatuses.AddRange(campaignStatuses);

            return masterData;
        }

        public async Task<CampaignsSummaryDTO> GetCampaignsSummaryAsync(CampaignFilterDTO campaignFilter)
        {
            var defaultCurrencyTypeId = tenantDbContext.AppSettings.FirstOrDefault(x => x.Name == CompanySettingConstants.DefaultCurrencyId);
            var defaultCurrencyType = await tenantDbContext.CurrencyTypes
                .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(defaultCurrencyTypeId.Value));
            // Reuse filtering logic from GetAllCampaignsAsync
            var query = tenantDbContext.Campaigns.Where(x => x.IsActive).AsQueryable();

            query = ApplyCampaignFilters(query, campaignFilter);

            var totalItems = await query.CountAsync();

            var page = campaignFilter.CurrentPage;
            var pageSize = campaignFilter.PageSize;
            var skip = (page - 1) * pageSize;

            var campaignsList = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Get all filtered campaigns
            var campaigns = await query
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new CampaignSummaryDTO
                {
                    Id = c.Id,
                    Title = c.Name,
                    Status = EnumHelper.GetEnumDescription(c.Status),
                    Description = c.Description ?? "",
                    TargetAmount = c.TargetAmount,
                    RaisedAmount = c.Donations.Sum(d => d.Amount),
                    StartDate = c.StartDate,
                    HasEndDate = c.HasEndDate,
                    EndDate = c.EndDate,
                    DonorCount = c.Donations.Select(d => d.DonorId).Distinct().Count(),
                    Category = c.CampaignCategory.Name,
                    Currency = c.CurrencyType.Name,
                    ImageUrl = c.CompaignImageUrl ?? "",
                    Visibility = EnumHelper.GetEnumDescription(c.Visibility),
                    DaysLeft = c.HasEndDate ? (c.EndDate.HasValue ? (int)(c.EndDate.Value - DateTime.UtcNow).TotalDays : 0) : 0
                })
                .ToListAsync();

            // Calculate statistics

            DateTime today = dateTime.Now;

            // Start of the month
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            // End of the month
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

            var statistics = new CampaignStatisticsDTO
            {
                TotalRaised = tenantDbContext.Donations
                .Sum(c => c.Amount)
                .ToString("N2", CultureInfo.InvariantCulture),
                ActiveCampaign = tenantDbContext.Campaigns.Count(cs => cs.Status == CampaignStatus.Active && cs.IsActive),
                ThisMonth = tenantDbContext.Donations.Where(x => x.Date <= endOfMonth && x.Date >= startOfMonth)
                .Sum(c => c.Amount)
                .ToString("N2", CultureInfo.InvariantCulture),
                TotalDonors = tenantDbContext.Donors.Count()
                // Add other statistics as needed
            };

            return new CampaignsSummaryDTO
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = totalItems,
                Statistics = statistics,
                Campaigns = campaigns,
                CurrencyType = defaultCurrencyType.Name,
            };
        }

        public async Task<ResultDto> UpdateCampaignAsync(CampaignDTO campaignDto)
        {
            // Validate input
            if (campaignDto.Id <= 0)
                return ResultDto.Failure(new[] { "Invalid campaign ID." });

            if (string.IsNullOrWhiteSpace(campaignDto.Name))
                return ResultDto.Failure(new[] { "Campaign name is required." });

            if (campaignDto.TargetAmount < 0)
                return ResultDto.Failure(new[] { "Target amount must be positive." });

            try
            {
                var campaign = await tenantDbContext.Campaigns.FindAsync(campaignDto.Id);
                if (campaign == null || !campaign.IsActive)
                    return ResultDto.Failure(new[] { "Campaign not found or is inactive." });

                // Update properties
                campaign.Name = campaignDto.Name;
                campaign.Description = campaignDto.Description;
                campaign.CampaignCategoryId = campaignDto.CampaignCategoryId;
                campaign.Visibility = campaignDto.Visibility;
                campaign.Status = campaignDto.Status;
                campaign.StartDate = campaignDto.StartDate.Value;
                campaign.HasEndDate = campaignDto.HasEndDate;
                campaign.EndDate = campaignDto.HasEndDate ? campaignDto.EndDate : null;
                campaign.CurrencyTypeId = campaignDto.CurrencyTypeId;
                campaign.CompaignImageUrl = campaignDto.CompaignImageUrl;
                campaign.TargetAmount = campaignDto.TargetAmount;
                campaign.UpdatedByUserId = currentUserService.UserId;
                campaign.UpdatedDate = dateTime.Now;

                await tenantDbContext.SaveChangesAsync();

                return ResultDto.Success("Campaign updated successfully.", campaign.Id);
            }
            catch (Exception ex)
            {
                return ResultDto.Failure(new[] { "Failed to update campaign.", ex.Message });
            }
        }

        private IQueryable<Campaign> ApplyCampaignFilters(IQueryable<Campaign> query, CampaignFilterDTO campaignFilter)
        {
            // Apply StatusId filter if not zero
            if (campaignFilter.StatusId != 0)
            {
                query = query.Where(c => (int)c.Status == campaignFilter.StatusId);
            }

            // Apply CategoryId filter if not zero
            if (campaignFilter.CategoryId != 0)
            {
                query = query.Where(c => c.CampaignCategoryId == campaignFilter.CategoryId);
            }

            // Apply SearchTerm filter if not null or whitespace
            if (!string.IsNullOrWhiteSpace(campaignFilter.SearchTerm))
            {
                var searchTerm = campaignFilter.SearchTerm.Trim().ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    (c.Description != null && c.Description.ToLower().Contains(searchTerm))
                );
            }

            // Only active campaigns
            query = query.Where(c => c.IsActive);

            return query;
        }

    }
}
