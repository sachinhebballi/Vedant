using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IConsultationLogic
    {
        void Save(ConsultationModel consultationModel);

        void SavePrescription(PrescriptionModel prescriptionModel);

        int GetLatestConsultation(int patientId);

        ConsultationResultModel GetAllConsultations(int patientId);

        ConsultationModel GetConsultation(int consultationId);

        void DeleteConsultation(int consultationId);
    }
}
