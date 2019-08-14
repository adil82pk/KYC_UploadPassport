using KYC_UploadPassportMicroService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KYC_UploadPassportMicroService.Helpers
{
    public static class StorageHelper
    {

        public static bool IsImage(IFormFile file)
        {
            try
            {
                if (file.ContentType.Contains("image"))
                {
                    return true;
                }

                string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

                return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<bool> UploadFileToStorage(Stream fileStream, string fileName, AzureStorageConfig _storageConfig)
        {
            try
            {
                // Create storagecredentials object by reading the values from the configuration (appsettings.json)
                StorageCredentials storageCredentials = new StorageCredentials(_storageConfig.AccountName, _storageConfig.AccountKey);

                // Create cloudstorage account by passing the storagecredentials
                CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = blobClient.GetContainerReference(_storageConfig.ImageContainer);

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                // Upload the file
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(true);
        }

        public static async Task<List<string>> GetThumbNailUrls(AzureStorageConfig _storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();

            try
            {
                // Create storagecredentials object by reading the values from the configuration (appsettings.json)
                StorageCredentials storageCredentials = new StorageCredentials(_storageConfig.AccountName, _storageConfig.AccountKey);

                // Create cloudstorage account by passing the storagecredentials
                CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

                // Create blob client
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the container
                CloudBlobContainer container = blobClient.GetContainerReference(_storageConfig.ThumbnailContainer);

                BlobContainerPermissions perm = await container.GetPermissionsAsync();
                perm.PublicAccess = BlobContainerPublicAccessType.Blob;
                await container.SetPermissionsAsync(perm);

                BlobContinuationToken continuationToken = null;

                BlobResultSegment resultSegment = null;

                //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
                //When the continuation token is null, the last page has been returned and execution can exit the loop.
                do
                {
                    //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                    //or by calling a different overload.
                    resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);

                    foreach (var blobItem in resultSegment.Results)
                    {
                        thumbnailUrls.Add(blobItem.StorageUri.PrimaryUri.ToString());
                    }

                    //Get the continuation token.
                    continuationToken = resultSegment.ContinuationToken;
                }

                while (continuationToken != null);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await Task.FromResult(thumbnailUrls);
        }
    }
}
