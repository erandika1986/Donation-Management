using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class UserService(
        TenantDbContext tenantDbContext,
        IDateTime dateTime,
        ICurrentUserService currentUserService) : IUserService
    {
        public async Task<ResultDto> DeleteAsync(int userId)
        {
            if (userId <= 0)
                return ResultDto.Failure(new[] { "A valid user ID is required." });

            var user = await tenantDbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return ResultDto.Failure(new[] { "User not found." });

            try
            {
                // Remove related user roles
                user.IsActive = false; // Soft delete
                user.UpdateDate = dateTime.UtcNow;
                user.UpdatedByUserId = currentUserService.UserId; // Assuming currentUserService provides the current user's ID

                tenantDbContext.Users.Update(user);
                await tenantDbContext.SaveChangesAsync();

                return ResultDto.Success("User deleted successfully.", userId);
            }
            catch (Exception)
            {
                // Log exception (not implemented)
                return ResultDto.Failure(new[] { "An error occurred while deleting the user." });
            }
        }

        public async Task<PaginatedResultDTO<UserDto>> GetAllAsync(UserFilterDTO filter)
        {
            var query = tenantDbContext.Users
                .Include(u => u.UserRoles)
                .AsQueryable();

            // Filter by search term (username, email, phone, fullname)
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(u =>
                    u.Username.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.Phone.ToLower().Contains(search) ||
                    u.FullName.ToLower().Contains(search));
            }

            // Filter by role if provided
            if (filter.RoleId > 0)
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == filter.RoleId));
            }

            var totalItems = await query.CountAsync();

            // Pagination
            var page = filter.CurrentPage > 0 ? filter.CurrentPage : 1;
            var pageSize = filter.PageSize > 0 ? filter.PageSize : 10;
            var skip = (page - 1) * pageSize;

            var users = await query
                .OrderBy(u => u.Username)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Phone = u.Phone,
                IsActive = u.IsActive,
                // If UserDto has AssignedRoles, map them as well
                AssignedRoles = u.UserRoles?.Select(ur => ur.RoleId).ToList()
            }).ToList();

            return new PaginatedResultDTO<UserDto>
            {
                Items = userDtos,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                IsReadOnly = true
            };
        }

        public async Task<UserDto> GetByIdAsync(int userId)
        {
            if (userId <= 0)
                return null;

            var user = await tenantDbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive,
                // If UserDto has AssignedRoles, map them as well
                AssignedRoles = user.UserRoles?.Select(ur => ur.RoleId).ToList()
            };

            return userDto;
        }

        public async Task<ResultDto> CreateAsync(RegisterDTO user)
        {
            try
            {
                if (user == null)
                    return ResultDto.Failure(new[] { "User data is required." });

                // Basic validation
                if (string.IsNullOrWhiteSpace(user.Username))
                    return ResultDto.Failure(new[] { "Email is required." });

                // Check if user already exists
                var existingUser = await tenantDbContext.Users
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == user.Username.ToLower());
                if (existingUser != null)
                    return ResultDto.Failure(new[] { "A user with this user name already exists." });

                // Create new user entity
                var newUser = new User
                {
                    // Map properties from RegisterDTO to User
                    Email = user.Email,
                    FullName = user.Fullname,
                    Phone = user.Phone,
                    Username = user.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password), // Ensure to hash the password in production code
                    IsActive = true,
                    CreatedDate = dateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId, // Assuming currentUserService provides the current user's ID
                    UpdateDate = dateTime.UtcNow,
                    UpdatedByUserId = currentUserService.UserId // Assuming currentUserService provides the current user's ID
                };

                // Assign roles if provided
                if (user.AssignedRoles != null && user.AssignedRoles.Any())
                {
                    foreach (var roleId in user.AssignedRoles)
                    {
                        newUser.UserRoles.Add(new UserRole
                        {
                            RoleId = roleId,
                            User = newUser
                        });
                    }
                }

                tenantDbContext.Users.Add(newUser);
                await tenantDbContext.SaveChangesAsync();

                return ResultDto.Success("User registered successfully.", newUser.Id);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return ResultDto.Failure(new[] { "An error occurred while creating the user." });
            }
        }

        public async Task<ResultDto> UpdateAsync(UserDto user)
        {
            if (user == null || user.Id <= 0)
                return ResultDto.Failure(new[] { "Valid user data is required." });

            var existingUser = await tenantDbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                return ResultDto.Failure(new[] { "User not found." });

            // Check for username/email conflicts
            if (!string.IsNullOrWhiteSpace(user.Username) &&
                !string.Equals(existingUser.Username, user.Username, StringComparison.OrdinalIgnoreCase))
            {
                var usernameExists = await tenantDbContext.Users
                    .AnyAsync(u => u.Username.ToLower() == user.Username.ToLower() && u.Id != user.Id);
                if (usernameExists)
                    return ResultDto.Failure(new[] { "A user with this user name already exists." });
                existingUser.Username = user.Username;
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
                existingUser.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(user.FullName))
                existingUser.FullName = user.FullName;

            if (!string.IsNullOrWhiteSpace(user.Phone))
                existingUser.Phone = user.Phone;

            existingUser.IsActive = user.IsActive;
            existingUser.UpdateDate = dateTime.UtcNow;
            existingUser.UpdatedByUserId = currentUserService.UserId;

            // Update roles
            if (user.AssignedRoles != null)
            {
                var currentRoleIds = existingUser.UserRoles.Select(ur => ur.RoleId).ToList();
                var newRoleIds = user.AssignedRoles.Distinct().ToList();

                // Remove roles not in new list
                var rolesToRemove = existingUser.UserRoles.Where(ur => !newRoleIds.Contains(ur.RoleId)).ToList();
                foreach (var ur in rolesToRemove)
                    tenantDbContext.UserRoles.Remove(ur);

                // Add new roles
                var rolesToAdd = newRoleIds.Except(currentRoleIds);
                foreach (var roleId in rolesToAdd)
                {
                    existingUser.UserRoles.Add(new UserRole
                    {
                        RoleId = roleId,
                        UserId = existingUser.Id
                    });
                }
            }

            try
            {
                await tenantDbContext.SaveChangesAsync();
                return ResultDto.Success("User updated successfully.", existingUser.Id);
            }
            catch (Exception)
            {
                // Log exception (not implemented)
                return ResultDto.Failure(new[] { "An error occurred while updating the user." });
            }
        }
    }
}
