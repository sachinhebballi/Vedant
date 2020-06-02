using System;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class ConsultationModel
    {
        public int PatientId { get; set; }
        public int ConsultationId { get; set; }
        public string Description { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string OthersPrescription { get; set; }
        public PatientModel Patient { get; set; }
        public List<string> ImageList { get; set; }
        public List<PrescriptionModel> Prescriptions { get; set; }
    }
}