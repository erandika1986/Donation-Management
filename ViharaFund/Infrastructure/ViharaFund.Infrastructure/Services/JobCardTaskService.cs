using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Application.Helpers;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.JobCardTask;

namespace ViharaFund.Infrastructure.Services
{
    public class JobCardTaskService(
        TenantDbContext tenantDbContext,
        IDateTime dateTime,
        ICurrentUserService currentUserService,
        IAzureBlobService azureBlobService,
        IConfiguration configuration) : IJobCardTaskService
    {
        public async Task<ResultDto> Create(JobCardTaskDTO jobCardTask)
        {
            if (jobCardTask == null)
            {
                return ResultDto.Failure(new[] { "Job card task cannot be null." });
            }

            var entity = new JobCardTask
            {
                JobCardId = jobCardTask.JobCardId,
                Title = jobCardTask.Title,
                Description = jobCardTask.Description,
                EstimateAmount = jobCardTask.EstimateAmount,
                TaskStatus = Domain.Enums.TaskStatus.Pending,
                CreatedDate = dateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                UpdatedDate = dateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,

                // Map other properties from DTO if available
            };

            entity.JobCardTaskComments.Add(new JobCardTaskComment()
            {
                Comment = jobCardTask.Comment,
                IsActive = true,
                CreatedDate = dateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                UpdatedDate = dateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId
            });

            await tenantDbContext.AddAsync(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task created successfully.", entity.Id);
        }

        public async Task<ResultDto> Delete(int id)
        {
            var entity = await tenantDbContext.Set<JobCardTask>().FindAsync(id);
            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job card task not found." });
            }

            entity.IsActive = false;
            entity.UpdatedDate = dateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            tenantDbContext.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task deleted successfully.", entity.Id);
        }

        public Task<ResultDto> DeleteJobCardTaskAttachment(int jobCardTaskId, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobCardTaskDTO>> GetAllByJobCardId(int jobCardId)
        {
            var defaultCurrencyTypeId = tenantDbContext.AppSettings.FirstOrDefault(x => x.Name == CompanySettingConstants.DefaultCurrencyId);
            var defaultCurrencyType = await tenantDbContext.CurrencyTypes
                .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(defaultCurrencyTypeId.Value));

            var tasks = await tenantDbContext.JobCardTasks
                .Where(t => t.JobCardId == jobCardId && t.IsActive)
                .Select(t => new JobCardTaskDTO
                {
                    Id = t.Id,
                    JobCardId = t.JobCardId,
                    JobCardTitle = t.JobCard != null ? t.JobCard.Title : null,
                    ActualAmount = t.ActualAmount,
                    EstimateAmount = t.EstimateAmount,
                    CurrencyType = defaultCurrencyType.Name,
                    TaskStatus = new DropDownDTO() { Id = (int)t.TaskStatus, Name = EnumHelper.GetEnumDescription(t.TaskStatus) },
                    Title = t.Title,
                    Description = t.Description,
                    StartDate = t.CreatedDate.ToString("yyyy-MM-dd"),
                    EndDate = t.CompletedDate.HasValue ? t.CompletedDate.Value.ToString("yyyy-MM-dd") : string.Empty
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<JobCardTaskDTO> GetById(int id)
        {
            var task = await tenantDbContext.JobCardTasks
                .Include(t => t.JobCard)
                .Where(t => t.Id == id && t.IsActive)
                .Select(t => new JobCardTaskDTO
                {
                    Id = t.Id,
                    JobCardId = t.JobCardId,
                    JobCardTitle = t.JobCard != null ? t.JobCard.Title : null,
                    ActualAmount = t.ActualAmount,
                    EstimateAmount = t.EstimateAmount,
                    TaskStatus = new DropDownDTO() { Id = (int)t.TaskStatus, Name = EnumHelper.GetEnumDescription(t.TaskStatus) },
                    Title = t.Title,
                    Description = t.Description
                })
                .FirstOrDefaultAsync();

            return task;
        }

        public async Task<TaskMasterDataDTO> GetTaskMasterData()
        {
            var masterData = new TaskMasterDataDTO();
            masterData.TaskStatuses.Add(new DropDownDTO() { Id = 0, Name = "All Status" });
            foreach (Domain.Enums.TaskStatus status in Enum.GetValues(typeof(Domain.Enums.TaskStatus)))
            {
                masterData.TaskStatuses.Add(new DropDownDTO
                {
                    Id = (int)status,
                    Name = EnumHelper.GetEnumDescription(status)
                });
            }

            return masterData;
        }

        public async Task<ResultDto> Update(JobCardTaskDTO jobCardTask)
        {
            if (jobCardTask == null || jobCardTask.Id == 0)
            {
                return ResultDto.Failure(new[] { "Invalid job card task data." });
            }

            var entity = await tenantDbContext.JobCardTasks
                .FirstOrDefaultAsync(t => t.Id == jobCardTask.Id && t.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job card task not found." });
            }

            // Update properties
            entity.Title = jobCardTask.Title;
            entity.JobCardId = jobCardTask.JobCardId;
            entity.EstimateAmount = jobCardTask.EstimateAmount;
            entity.ActualAmount = jobCardTask.ActualAmount;
            entity.TaskStatus = (ViharaFund.Domain.Enums.TaskStatus)jobCardTask.TaskStatus.Id;
            entity.UpdatedDate = dateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            entity.JobCardTaskComments.Add(new JobCardTaskComment()
            {
                Comment = jobCardTask.Comment,
                IsActive = true,
                CreatedDate = dateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                UpdatedDate = dateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId
            });

            // Map other updatable properties from DTO if available

            tenantDbContext.JobCardTasks.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task updated successfully.", entity.Id);
        }

        public async Task<ResultDto> UpdateJobCardTaskStatus(int jobCardTaskId, string comment, Domain.Enums.TaskStatus taskStatus)
        {
            // Step 1: Find the JobCardTask entity by id and ensure it's active
            var entity = await tenantDbContext.JobCardTasks
                .FirstOrDefaultAsync(t => t.Id == jobCardTaskId && t.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job card task not found." });
            }

            // Step 2: Update the status
            entity.TaskStatus = taskStatus;
            entity.UpdatedDate = dateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            // Step 3: Add a comment if provided
            if (!string.IsNullOrWhiteSpace(comment))
            {
                entity.JobCardTaskComments.Add(new JobCardTaskComment
                {
                    Comment = comment,
                    IsActive = true,
                    CreatedDate = dateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId,
                    UpdatedDate = dateTime.UtcNow,
                    UpdatedByUserId = currentUserService.UserId
                });
            }

            // Step 4: Save changes
            tenantDbContext.JobCardTasks.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            // Step 5: Return success
            return ResultDto.Success("Job card task status updated successfully.", entity.Id);
        }


        public async Task<ResultDto> UploadJobCardTaskAttachment(UploadFileDTO upload)
        {
            try
            {
                string leaveSupportDocumentPath = configuration["FileSavePaths:LeaveSupportDocumentPath"];
                if (!Directory.Exists(leaveSupportDocumentPath))
                {
                    Directory.CreateDirectory(leaveSupportDocumentPath);
                }


                for (int i = 0; i < upload.Files.Count; i++)
                {
                    var extension = Path.GetExtension(upload.Files[i].Name);

                    var fileName = $"{Path.GetFileNameWithoutExtension(upload.Files[i].Name)}{extension}";
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(leaveSupportDocumentPath, uniqueFileName);

                    await using var stream = upload.Files[i].OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var uploadedFileUrl = await azureBlobService
                        .UploadFileAsync(memoryStream, uniqueFileName, upload.Files[i].ContentType, ApplicationConstants.AzureBlobStorageName);

                    var jobCardTaskAttachment = new JobCardTaskAttachment()
                    {
                        JobCardTaskId = upload.JobCardTaskId,
                        FileName = fileName,
                        FilePath = uploadedFileUrl,
                        CreatedDate = dateTime.UtcNow,
                        CreatedByUserId = currentUserService.UserId,
                        UpdatedDate = dateTime.UtcNow,
                        UpdatedByUserId = currentUserService.UserId,
                        IsActive = true,
                    };

                    await tenantDbContext.JobCardTaskAttachments.AddAsync(jobCardTaskAttachment);

                }

                await tenantDbContext.SaveChangesAsync();

                return ResultDto.Success("File(s) uploaded successfully.");
            }
            catch (Exception ex)
            {
                return ResultDto.Failure(new[] { "An error occurred while uploading the file.", ex.Message });
            }
        }
    }
}
