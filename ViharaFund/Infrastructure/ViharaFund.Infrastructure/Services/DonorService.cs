using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class DonorService(TenantDbContext tenantDbContext) : IDonorService
    {
        public async Task<ResultDto> DeleteAsync(int donorId)
        {
            var entity = await tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donorId);
            if (entity == null)
                return ResultDto.Failure(new[] { "Donor not found." });

            // Soft delete
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;

            tenantDbContext.Donors.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Donor deleted successfully", entity.Id);
        }

        public async Task<PaginatedResultDTO<DonorDTO>> GetAllAsync(DonorFilterDTO filter)
        {
            var query = tenantDbContext.Donors.AsQueryable();

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

        public async Task<DonorDTO> GetByIdAsync(int donorId)
        {
            var entity = await tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donorId && x.IsActive);
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

        public async Task<ResultDto> saveAsync(DonorDTO donor)
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
                tenantDbContext.Donors.Add(entity);
            }
            else
            {
                entity = await tenantDbContext.Donors.FirstOrDefaultAsync(x => x.Id == donor.Id);
                if (entity == null)
                    return ResultDto.Failure(new[] { "Donor not found." });

                entity.Name = donor.Name;
                entity.Email = donor.Email;
                entity.Phone = donor.Phone;
                entity.Address = donor.Address;
                entity.UpdatedDate = DateTime.UtcNow;

                tenantDbContext.Donors.Update(entity);
            }

            await tenantDbContext.SaveChangesAsync();
            return ResultDto.Success("Donor saved successfully", entity.Id);
        }

        public async Task<List<DropDownDTO>> SearchAsync(string searchText)
        {
            return await tenantDbContext.Donors
                .Where(x => x.IsActive &&
                            (x.Name.Contains(searchText) ||
                             x.Email.Contains(searchText) ||
                             x.Phone.Contains(searchText)))
                .Select(x => new DropDownDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}
