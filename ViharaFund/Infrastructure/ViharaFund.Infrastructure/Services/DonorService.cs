using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
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

        public async Task<ResultDto> DeleteDonorAsync(int donorId)
        {
            var entity = await _tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donorId);
            if (entity == null)
                return ResultDto.Failure(new[] { "Donor not found." });

            // Soft delete
            entity.IsActive = false;
            entity.UpdateDate = DateTime.UtcNow;

            _tenantDbContext.Donors.Update(entity);
            await _tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Donor deleted successfully", entity.Id);
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

        public async Task<DonorDTO> GetDonorByIdAsync(int donorId)
        {
            var entity = await _tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donorId && x.IsActive);
            if (entity == null)
                return null;

            return new DonorDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address
            };
        }

        public async Task<ResultDto> SaveDonorAsync(DonorDTO donor)
        {
            if (string.IsNullOrWhiteSpace(donor.Name))
            {
                return ResultDto.Failure(new[] { "Donor name is required." });
            }

            Donor entity;
            if (donor.Id == 0)
            {
                entity = new Donor
                {
                    Name = donor.Name,
                    Email = donor.Email,
                    Phone = donor.Phone,
                    Address = donor.Address,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                _tenantDbContext.Donors.Add(entity);
            }
            else
            {
                entity = await _tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donor.Id);
                if (entity == null)
                    return ResultDto.Failure(new[] { "Donor not found." });

                entity.Name = donor.Name;
                entity.Email = donor.Email;
                entity.Phone = donor.Phone;
                entity.Address = donor.Address;
                entity.UpdateDate = DateTime.UtcNow;

                _tenantDbContext.Donors.Update(entity);
            }

            await _tenantDbContext.SaveChangesAsync();
            return ResultDto.Success("Donor saved successfully", entity.Id);
        }

        public async Task<List<DropDownDto>> SearchDonorAsync(string searchText)
        {
            return await _tenantDbContext.Donors
                .Where(x => x.IsActive &&
                            (x.Name.Contains(searchText) ||
                             x.Email.Contains(searchText) ||
                             x.Phone.Contains(searchText)))
                .Select(x => new DropDownDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}
