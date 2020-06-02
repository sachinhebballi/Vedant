using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class CentreModel
    {
        public int CentreId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CentreName { get; set; }
        public string CentreCode { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedById { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Logo { get; set; }
        public string ContactNumber { get; set; }
        public string ZipCode { get; set; }
        public string DoctorName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> ImageList { get; set; }
    }
}