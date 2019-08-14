using KYC_UploadPassportMicroService.Models;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;
using Moq;
using KYC_UploadPassportMicroService.Helpers;

namespace HYC_UploadPassportMicroServiceTest
{
    public class KYCUnitTest
    {
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private AzureStorageConfig storageConfig = new AzureStorageConfig()
        {
            AccountName = "kycstorage",
            AccountKey = "txbeZLRldWh31kQt4XQBF+eZGITcedseHTTAz39DzdO63wZE103/cl0YRolQME3xcNR2wmtqnJCViqZVYjhhmw==",
            ImageContainer = "kyccontainer",
            ThumbnailContainer = "kyccontainer"
        };

        /// <summary>
        /// Test when upload image to blob is Successful
        /// </summary>
        [Fact]
        public async void Test_When_UploadImageToBlob_Successful()
        {
            byte[] mybytes = File.ReadAllBytes(@"D:\aus.jpg");
            Stream stream = new MemoryStream(mybytes);

            var result = await StorageHelper.UploadFileToStorage(stream, "passport", storageConfig);
            Assert.True(result);
        }

        /// <summary>
        /// Test when upload image format is invalid
        /// </summary>
        [Fact]
        public void Test_When_UploadImageFormatIsInvalid()
        {
            var result = StorageHelper.IsImage(GetFormFile("Image", "passport.tiff"));
            Assert.False(result);
        }

        /// <summary>
        /// Test when upload image in JpegForm is true
        /// </summary>
        [Fact]
        public void Test_When_UploadImageInJpegForm_Is_True()
        {
            var result = StorageHelper.IsImage(GetFormFile("Image", "passport.jpg"));
            Assert.True(result);
        }


        /// <summary>
        /// Test when thumbnail Urls count is greater than ZERO
        /// </summary>
        [Fact]
        public async void Test_When_ThumbNailUrlsCount_IsGreaterThanZERO()
        {
            var result = await StorageHelper.GetThumbNailUrls(storageConfig);
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// Get Form File object
        /// </summary>
        /// <param name="ContentType">content type (Image etc)</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        private IFormFile GetFormFile(string ContentType, string fileName)
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Passport image mock";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.ContentType).Returns(ContentType);

            return fileMock.Object;
        }
    }
}
