using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Application.Services;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class DonorService : IDonorService
    {
        private readonly TenantDbContext _tenantDbContext;
        public DonorService(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }
        public async Task<PaginatedResultDTO<DonorDTO>> GetAllDonorsAsync(DonorFilterDTO filter)
        {
            var query = _tenantDbContext.Donors.AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(d => d.Name.Contains(filter.SearchTerm) || d.Email.Contains(filter.SearchTerm) || d.Phone.Contains(filter.SearchTerm));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new DonorDTO()
                {
                    Address = x.Address,
                    Email = x.Email,
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone

                }).ToListAsync();

            var newResult = new PaginatedResultDTO<DonorDTO>
            {
                Items = items,
                TotalItems = totalCount,
                Page = filter.CurrentPage,
                PageSize = filter.PageSize
            };

            return newResult;
        }
    }
}
