namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Prescription
    {
        public int PrescriptionId { get; set; }
        public int ConsultationId { get; set; }
        public int DrugId { get; set; }
        public string Dosage { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public int DosageTiming { get; set; }
        public virtual Consultation Consultation { get; set; }        
    }
}
