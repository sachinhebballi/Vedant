using System;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class PatientModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public byte? Age { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public int RegisteredCentreId { get; set; }
        public string RegisteredCentre { get; set; }
        public string RegistrationNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Consultations { get; set; }
    }
}