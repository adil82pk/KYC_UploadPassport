using System;
using System.Collections.Generic;

namespace KYC_UploadPassportMicroService.Models
{
    public partial class Transaction
    {
        public string TransactionId { get; set; }
        public string RecordStatus { get; set; }
        public string CountryCode { get; set; }
        public string AbbyyCloudOcrresponseId { get; set; }

        public AbbyyCloudOcrresponse AbbyyCloudOcrresponse { get; set; }
    }
}
