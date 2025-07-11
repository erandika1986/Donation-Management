﻿using Azure.Storage.Blobs.Models;

namespace ViharaFund.Application.Services
{
    public interface IAzureBlobService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string containerName, PublicAccessType publicAccessType = PublicAccessType.None);
        Task<BlobDownloadInfo> DownloadFileAsync(string blobName, string containerName);
        Task DeleteFileAsync(string blobName, string containerName);
        Task<string> GenerateSasTokenForBlobAsync(string blobName, string containerName, DateTimeOffset expiresOn);
    }
}
