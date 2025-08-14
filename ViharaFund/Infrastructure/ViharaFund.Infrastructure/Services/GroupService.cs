using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.Group;

namespace ViharaFund.Infrastructure.Services
{
    public class GroupService(TenantDbContext tenantDbContext, ICurrentUserService currentUserService, IDateTime dateTime) : IGroupService
    {
        public async Task<ResultDto> CreateGroup(GroupDTO group)
        {
            var newGroup = new Group()
            {
                Name = group.Name,
                RoleId = group.SelectedRole.Id,
                IsActive = true,
                CreatedByUserId = currentUserService.UserId,
                UpdatedByUserId = currentUserService.UserId,
                CreatedDate = dateTime.UtcNow,
                UpdatedDate = dateTime.UtcNow
            };

            foreach (var user in group.Users)
            {
                newGroup.GroupUsers.Add(new GroupUser()
                {
                    UserId = user.Id
                });
            }

            tenantDbContext.Groups.Add(newGroup);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Group created successfully.");
        }

        public async Task<ResultDto> DeleteSelectedGroup(int groupId)
        {
            var group = await tenantDbContext.Groups.FindAsync(groupId);
            foreach (var groupUser in group.GroupUsers.ToList())
            {
                tenantDbContext.GroupUsers.Remove(groupUser);
            }

            await tenantDbContext.SaveChangesAsync();

            tenantDbContext.Groups.Remove(group);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Group deleted successfully.");
        }

        public async Task<List<GroupDTO>> GetAllGroups()
        {
            var groups = await tenantDbContext.Groups
                .Select(g => new GroupDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    SelectedRole = new DropDownDTO() { Id = g.RoleId, Name = g.Role.Name },
                    //UserCount = g.GroupUsers.Count,
                    Users = g.GroupUsers.Select(gu => new DropDownDTO
                    {
                        Id = gu.UserId,
                        Name = gu.User.FullName
                    }).ToList()
                })
                .ToListAsync();

            return groups;
        }

        public async Task<ResultDto> UpdateGroup(GroupDTO group)
        {
            // Step 1: Find the existing group by ID (assume group.Id is present in DTO)
            var existingGroup = await tenantDbContext.Groups
                .Include(g => g.GroupUsers)
                .FirstOrDefaultAsync(g => g.Id == group.Id);

            if (existingGroup == null)
            {
                return ResultDto.Failure(new[] { "Group not found." });
            }

            // Step 2: Update group properties
            existingGroup.Name = group.Name;
            existingGroup.RoleId = group.SelectedRole.Id;
            existingGroup.UpdatedByUserId = currentUserService.UserId;
            existingGroup.UpdatedDate = dateTime.UtcNow;

            // Step 3: Update group users
            var newUserIds = group.Users.Select(u => u.Id).ToList();
            var existingUserIds = existingGroup.GroupUsers.Select(gu => gu.UserId).ToList();

            // Remove users not in new list
            foreach (var groupUser in existingGroup.GroupUsers.Where(gu => !newUserIds.Contains(gu.UserId)).ToList())
            {
                tenantDbContext.GroupUsers.Remove(groupUser);
            }

            // Add new users not in existing list
            foreach (var userId in newUserIds.Except(existingUserIds))
            {
                existingGroup.GroupUsers.Add(new GroupUser
                {
                    UserId = userId
                });
            }

            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Group updated successfully.");
        }
    }
}
