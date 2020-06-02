using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class ConsultationResultModel
    {
        public List<ConsultationModel> ConsultationModel { get; set; }
        public PatientModel PatientModel { get; set; }
    }
}