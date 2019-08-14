using System;
using System.Collections.Generic;

namespace KYC_UploadPassportMicroService.Models
{
    public partial class AbbyyCloudOcrresponse
    {
        public AbbyyCloudOcrresponse()
        {
            Transaction = new HashSet<Transaction>();
        }

        public string MrzType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public int? Checksum { get; set; }
        public bool? ChecksumVerified { get; set; }
        public string DocumentType { get; set; }
        public string DocumentSubtype { get; set; }
        public string IssuingCountry { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string DocumentNumber { get; set; }
        public bool? DocumentNumberVerified { get; set; }
        public string DocumentNumberCheck { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
        public bool? BirthDateVerified { get; set; }
        public string BirthDateCheck { get; set; }
        public string Sex { get; set; }
        public string ExpiryDate { get; set; }
        public bool? ExpiryDateVerified { get; set; }
        public string ExpiryDateCheck { get; set; }
        public string PersonalNumber { get; set; }
        public bool? PersonalNumberVerified { get; set; }
        public string PersonalNumberCheck { get; set; }
        public string Id { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
    }
}
