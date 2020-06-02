namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class PrescriptionModel
    {
        public int ConsultationId { get; set; }
        public int PrescriptionId { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string MeasureUnit { get; set; }
        public string Dosage { get; set; }
        public int DosageTiming { get; set; }
    }
}