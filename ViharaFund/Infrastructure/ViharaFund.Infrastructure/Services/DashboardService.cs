using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.Helpers;
using ViharaFund.Application.Services;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Dashboard;

namespace ViharaFund.Infrastructure.Services
{
    public class DashboardService(
        TenantDbContext tenantDbContext,
        IDateTime dateTime,
        ICurrentUserService currentUserService) : IDashboardService
    {
        public async Task<CampaignDashboardDTO> GetCampaignDashboard()
        {
            var response = new CampaignDashboardDTO();

            var campaigns = await tenantDbContext.Campaigns
                .Where(c => c.IsActive)
                .ToListAsync();

            response.ActiveCampaigns = campaigns.Count;

            var startEndDateOfMonth = GetMonthStartDateEndDate(dateTime.Now);
            response.NewInThisMonth = await tenantDbContext.Campaigns
                .CountAsync(c => c.CreatedDate >= startEndDateOfMonth.Item1 && c.CreatedDate <= startEndDateOfMonth.Item2);

            foreach (var campaign in campaigns)
            {
                var actualIncome = campaign.Donations.Where(d => d.IsActive).Sum(x => x.Amount);
                var performance = new CampaignPerformanceDTO
                {
                    CampaignName = campaign.Name,
                    Status = EnumHelper.GetEnumDescription(campaign.Status),
                    TargetProgress = campaign.TargetAmount > 0 ? ((double)actualIncome / (double)campaign.TargetAmount) * 100 : 0,
                    TargetAmount = campaign.TargetAmount,
                    ActualIncome = actualIncome
                };
                response.CampaignPerformances.Add(performance);
            }

            return response;

        }

        public async Task<DonationDashboardDTO> GetDonationDashboard()
        {
            var response = new DonationDashboardDTO();

            var query = tenantDbContext.Donations
                .Where(d => d.IsActive);

            var startEndDateOfMonth = GetMonthStartDateEndDate(dateTime.Now);

            var startOfLastMonth = new DateTime(dateTime.Now.Year, dateTime.Now.Month, 1).AddMonths(-1);
            var endOfLastMonth = new DateTime(dateTime.Now.Year, dateTime.Now.Month, 1).AddSeconds(-1);

            var donationThisMonth = query
                .Where(d => d.IsActive && d.CreatedDate >= startEndDateOfMonth.Item1 && d.CreatedDate <= startEndDateOfMonth.Item2)
                .Sum(x => x.Amount);

            var donationLastMonth = query
                .Where(d => d.IsActive && d.CreatedDate >= startOfLastMonth && d.CreatedDate <= endOfLastMonth)
                .Sum(x => x.Amount);

            response.TotalDonation = donationThisMonth;
            response.MonthlyProgress = donationLastMonth > 0 ? (donationThisMonth - donationLastMonth) / donationLastMonth * 100 : 0;


            return response;
        }

        public async Task<DonorDashboardDTO> GetDonorDashboard()
        {
            var response = new DonorDashboardDTO();

            var startEndDateOfMonth = GetMonthStartDateEndDate(dateTime.Now);

            var donors = tenantDbContext.Donors.Where(x => x.IsActive);

            response.TotalDonor = await donors.CountAsync();

            response.NewInThisMonth = await donors.CountAsync(x => x.CreatedDate >= startEndDateOfMonth.Item1 && x.CreatedDate <= startEndDateOfMonth.Item2);

            var topToDonors = await tenantDbContext.Donations.GroupBy(x => x.DonorId)
                .Select(g => new
                {
                    DonorId = g.Key,
                    TotalDonation = g.Sum(x => x.Amount),
                    NoOfDonation = g.Count()
                })
                .OrderByDescending(x => x.TotalDonation)
                .Take(10)
                .Join(donors, d => d.DonorId, donor => donor.Id, (d, donor) => new TopDonorsSummaryDTO
                {
                    DonorName = donor.Name,
                    NumberOfDonation = d.NoOfDonation,
                    TotalDonation = d.TotalDonation
                }).ToListAsync();

            response.TopDonors.AddRange(topToDonors);
            return response;
        }

        public async Task<JobDashboardDTO> GetJobDashboard()
        {
            var response = new JobDashboardDTO();
            var jobCards = tenantDbContext.JobCards.Where(x => x.IsActive);
            response.TotalPendingJobs = await jobCards.CountAsync(x => x.Status == Domain.Enums.JobCardStatus.OnGoing);
            response.TotalTask = await jobCards.CountAsync(x => x.Priority == Domain.Enums.JobPriority.Emergency || x.Priority == Domain.Enums.JobPriority.Critical);

            foreach (Domain.Enums.JobCardStatus status in (Domain.Enums.JobCardStatus[])Enum.GetValues(typeof(Domain.Enums.JobCardStatus)))
            {
                var matchingJobCards = jobCards.Where(x => x.Status == status);
                response.JobStatusSummary.Add(new JobStatusSummaryDTO()
                {
                    JobCardCount = matchingJobCards.Count(),
                    Status = status,
                    StatusText = EnumHelper.GetEnumDescription(status),
                    Precentage = jobCards.Count() > 0 ? ((double)matchingJobCards.Count() / (double)jobCards.Count()) * 100 : 0
                });
            }

            var recentJobCards = await jobCards
                .OrderByDescending(j => j.UpdatedDate)
                .Take(5)
                .Select(x => new RecentJobCardSummaryDTO()
                {
                    Id = x.Id,
                    Name = x.Title,
                    Status = EnumHelper.GetEnumDescription(x.Status),
                    OwnedGroup = x.AssignGroup.Name
                }).ToListAsync();

            response.RecentJobCards.AddRange(recentJobCards);

            return response;
        }

        public Tuple<DateTime, DateTime> GetMonthStartDateEndDate(DateTime date)
        {
            var startDate = new DateTime(date.Year, date.Month, 1);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);
            return Tuple.Create(startDate, endDate);
        }
    }
}
