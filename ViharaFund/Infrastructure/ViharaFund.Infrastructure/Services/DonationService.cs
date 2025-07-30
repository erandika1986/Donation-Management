using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donation;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class DonationService(
        TenantDbContext tenantDbContext,
        IDonorService donorService,
        ICurrentUserService currentUserService) : IDonationService
    {
        public async Task<ResultDto> DeleteAsync(int donationId)
        {
            var donation = await tenantDbContext.Donations.FindAsync(donationId);
            if (donation == null)
                return ResultDto.Success("Donation not found", 0);

            donation.IsActive = false;
            donation.UpdatedDate = DateTime.UtcNow;

            tenantDbContext.Donations.Update(donation);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Donation deleted successfully", donationId);
        }

        public async Task<PaginatedResultDTO<DonationSummaryDTO>> GetAllAsync(DonationFilterDTO filter)
        {
            var query = tenantDbContext.Donations
                .Include(d => d.Donor)
                .Include(d => d.Campaign)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Donor.Name.Contains(filter.SearchTerm) || x.Campaign.Name.Contains(filter.SearchTerm));
            }

            var totalItems = await query.CountAsync();

            var donations = await query
                .OrderByDescending(d => d.Date)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(d => new DonationSummaryDTO
                {
                    Id = d.Id,
                    DonorName = d.Donor != null ? d.Donor.Name : "",
                    CampaignName = d.Campaign != null ? d.Campaign.Name : "",
                    Amount = d.Amount,
                    Date = d.Date.ToString("yyyy-MM-dd"),
                    Note = d.Note
                })
                .ToListAsync();

            return new PaginatedResultDTO<DonationSummaryDTO>
            {
                TotalItems = totalItems,
                Page = filter.CurrentPage,
                PageSize = filter.PageSize,
                Items = donations
            };
        }

        public async Task<DonationDTO> GetByIdAsync(int donationId)
        {
            var donation = await tenantDbContext.Donations.AsNoTracking().FirstOrDefaultAsync(d => d.Id == donationId);
            if (donation == null)
                return null;

            return new DonationDTO
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                CampaignId = donation.CampaignId,
                Amount = donation.Amount,
                Date = donation.Date,
                Note = donation.Note
            };
        }

        public async Task<DonationMasterDataDTO> GetDonationMasterDataAsync()
        {
            var masterData = new DonationMasterDataDTO();

            var donorPurposes = await tenantDbContext.Campaigns.Where(a => a.IsActive)
                .OrderBy(dp => dp.Name)
                .Select(dp => new DropDownDTO
                {
                    Id = dp.Id,
                    Name = dp.Name
                })
                .ToListAsync();

            masterData.Campaigns.AddRange(donorPurposes);

            return masterData;
        }

        public async Task<List<DonationSummaryDTO>> GetRecentRecordsAsync(int numberOfRecords)
        {
            var donations = await tenantDbContext.Donations
                .Include(d => d.Donor)
                .Include(d => d.Campaign)
                .OrderByDescending(d => d.Date)
                .Take(numberOfRecords)
                .Select(d => new DonationSummaryDTO
                {
                    Id = d.Id,
                    DonorName = d.Donor != null ? d.Donor.Name : "",
                    CampaignName = d.Campaign != null ? d.Campaign.Name : "",
                    Amount = d.Amount,
                    Date = d.Date.ToString("yyyy-MM-dd"),
                    Note = d.Note
                })
                .ToListAsync();

            return donations;
        }

        public async Task<ResultDto> SaveAsync(DonationDTO donation)
        {
            Donation? entity; // Use nullable type to handle potential null values

            //var result = await donorService.SaveAsync(donation.Donor);
            //if (!result.Succeeded)
            //    return ResultDto.Failure(new List<string>() { $"Failed to save donor information : {string.Join(", ", result)}" });
            donation.DonorId = donation.DonorId;

            if (donation.Id > 0)
            {
                entity = await tenantDbContext.Donations.FindAsync(donation.Id);
                if (entity == null)
                    return ResultDto.Success("Donation not found", 0);

                entity.DonorId = donation.DonorId;
                entity.CampaignId = donation.CampaignId;
                entity.Amount = donation.Amount;
                //entity.Date = DateTime.ParseExact(donation.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                entity.Date = donation.Date.Value;
                entity.Note = donation.Note;
                entity.UpdatedByUserId = currentUserService.UserId;
                entity.UpdatedDate = DateTime.UtcNow;
                tenantDbContext.Donations.Update(entity);
            }
            else
            {
                entity = new Donation
                {
                    DonorId = donation.DonorId,
                    CampaignId = donation.CampaignId,
                    Amount = donation.Amount,
                    //Date = DateTime.ParseExact(donation.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Date = donation.Date.Value,
                    Note = donation.Note,
                    CreatedByUserId = currentUserService.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    UpdatedByUserId = currentUserService.UserId,
                    UpdatedDate = DateTime.UtcNow
                };
                await tenantDbContext.Donations.AddAsync(entity);
            }

            await tenantDbContext.SaveChangesAsync();
            return ResultDto.Success("Donation saved successfully", entity.Id);
        }
    }
}
