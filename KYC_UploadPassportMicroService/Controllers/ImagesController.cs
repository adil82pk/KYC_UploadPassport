using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KYC_UploadPassportMicroService.Helpers;
using KYC_UploadPassportMicroService.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KYC_UploadPassportMicroService.Controllers
{
    /// <summary>
    /// Inage controller
    /// </summary>
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private readonly AzureStorageConfig storageConfig = null;
        private TelemetryClient _insightsClient;

        /// <summary>
        /// ImagesController contructor
        /// </summary>
        /// <param name="config">azure storage settings</param>
        /// <param name="insightsClient">log errors in application insight</param>
        public ImagesController(IOptions<AzureStorageConfig> config, TelemetryClient insightsClient)
        {
            storageConfig = config.Value;
            _insightsClient = insightsClient;
        }

        /// <summary>
        /// Upload images to Azure blob
        /// POST /api/images/upload 
        /// </summary>
        /// <param name="files">Collection of files</param>
        /// <returns>ActionResult</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            bool isUploaded = false;

            try
            {
                if (files.Count == 0)
                {
                    return BadRequest("No files received from the upload");
                }

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using (Stream stream = formFile.OpenReadStream())
                            {
                                isUploaded = await StorageHelper.UploadFileToStorage(stream, formFile.FileName, storageConfig);
                            }
                        }
                    }
                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }

                if (isUploaded)
                {
                    if (storageConfig.ThumbnailContainer != string.Empty)
                    {
                        return new AcceptedAtActionResult("GetThumbNails", "Images", null, null);
                    }
                    else
                    {
                        return new AcceptedResult();
                    }
                }
                else
                {
                    return BadRequest("Look like the image couldnt upload to the storage");
                }
            }
            catch (Exception ex)
            {
                _insightsClient.TrackException(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Grt thumbnail from Azure blob 
        ///  GET /api/images/thumbnails
        /// </summary>
        /// <returns>ActionResult</returns> 
        [HttpGet("thumbnails")]
        public async Task<IActionResult> GetThumbNails()
        {
            try
            {
                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(storageConfig);
                return new ObjectResult(thumbnailUrls);
            }
            catch (Exception ex)
            {
                _insightsClient.TrackException(ex);

            }
            return null;
        }
    }
}