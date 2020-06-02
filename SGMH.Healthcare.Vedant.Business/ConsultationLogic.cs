using Microsoft.EntityFrameworkCore;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;
using System;
using System.Linq;

namespace SGMH.Healthcare.Vedant.Business
{
    public class ConsultationLogic : IConsultationLogic
    {
        private readonly PatientsContext _patientsContext;
        public ConsultationLogic(PatientsContext patientsContext)
        {
            _patientsContext = patientsContext;
        }

        public void Save(ConsultationModel consultationModel)
        {
            if (consultationModel.ConsultationId == 0)
            {
                var consultationEntity = new Consultation
                {
                    PatientId = consultationModel.PatientId,
                    Description = consultationModel.Description,
                    ModifiedDate = DateTime.Now.ToLocalTime(),
                    OthersPrescription = consultationModel.OthersPrescription,
                    ConsultationDate = DateTime.Now.ToLocalTime()
                };

                _patientsContext.Consultation.Add(consultationEntity);
                Save();

                foreach (var prescriptionEntity in from prescription in consultationModel.Prescriptions
                                                   let drug = _patientsContext.Drug.FirstOrDefault(d => d.DrugName == prescription.DrugName)
                                                   where drug != null
                                                   select new Prescription
                                                   {
                                                       ConsultationId = consultationEntity.ConsultationId,
                                                       DrugId = drug.DrugId,
                                                       Dosage = prescription.Dosage,
                                                       DosageTiming = prescription.DosageTiming,
                                                       Consultation = consultationEntity,
                                                       ModifiedDate = DateTime.Now.ToLocalTime()
                                                   })
                {
                    _patientsContext.Prescription.Add(prescriptionEntity);
                }

                if (consultationModel.ImageList != null)
                {
                    foreach (var prescriptionImage in consultationModel.ImageList.Select(image => new PrescriptionImage
                    {
                        ConsultationId = consultationEntity.ConsultationId,
                        PrescriptionImageName = image
                    }))
                    {
                        _patientsContext.PrescriptionImages.Add(prescriptionImage);
                    }
                }
            }
            else
            {
                var consultationEntity = _patientsContext.Consultation.FirstOrDefault(c => c.ConsultationId == consultationModel.ConsultationId);
                if (consultationEntity != null)
                {
                    consultationEntity.Description = consultationModel.Description;
                    consultationEntity.OthersPrescription = consultationModel.OthersPrescription;
                    consultationEntity.ModifiedDate = DateTime.Now.ToLocalTime();

                    var prescriptionIds = consultationModel.Prescriptions
                        .Where(p => p.PrescriptionId != 0)
                        .Select(p => p.PrescriptionId)
                        .ToList();

                    var existingPrescriptionIds = _patientsContext.Prescription
                        .Where(p => p.ConsultationId == consultationModel.ConsultationId)
                        .Select(p => p.PrescriptionId).ToList();

                    if (prescriptionIds.Count != existingPrescriptionIds.Count)
                    {
                        var toBeDeleted = existingPrescriptionIds.Except(prescriptionIds).ToList();
                        foreach (var prescription in toBeDeleted
                            .Select(id => _patientsContext.Prescription
                            .FirstOrDefault(p => p.PrescriptionId == id)))
                        {
                            if (prescription != null) _patientsContext.Prescription.Remove(prescription);
                        }
                    }

                    foreach (var prescription in consultationModel.Prescriptions)
                    {
                        if (prescription.PrescriptionId == 0)
                        {
                            var drug = _patientsContext.Drug
                                .FirstOrDefault(d => d.DrugName == prescription.DrugName);

                            if (drug == null) continue;
                            var prescriptionEntity = new Prescription
                            {
                                ConsultationId = consultationModel.ConsultationId,
                                DrugId = drug.DrugId,
                                Dosage = prescription.Dosage,
                                DosageTiming = prescription.DosageTiming,
                                Consultation = consultationEntity,
                                ModifiedDate = DateTime.Now.ToLocalTime()
                            };

                            _patientsContext.Prescription.Add(prescriptionEntity);
                        }
                        else
                        {
                            var p = _patientsContext.Prescription.FirstOrDefault(pp => pp.PrescriptionId == prescription.PrescriptionId);
                            if (p != null)
                            {
                                p.DrugId = prescription.DrugId;
                                p.Dosage = prescription.Dosage;
                                p.DosageTiming = prescription.DosageTiming;
                                p.ModifiedDate = DateTime.Now.ToLocalTime();
                            }
                        }
                    }
                }

                if (consultationModel.ImageList != null)
                {
                    foreach (var prescriptionImage in consultationModel.ImageList.Select(image => new PrescriptionImage
                    {
                        ConsultationId = consultationModel.ConsultationId,
                        PrescriptionImageName = image
                    }))
                    {
                        _patientsContext.PrescriptionImages.Add(prescriptionImage);
                    }
                }
            }

            Save();
        }

        public void SavePrescription(PrescriptionModel prescriptionModel)
        {
            var prescriptionEntity = _patientsContext.Prescription.FirstOrDefault(p => p.ConsultationId == prescriptionModel.ConsultationId);
            if (prescriptionEntity != null)
            {
                prescriptionEntity.DrugId = prescriptionModel.DrugId;
                prescriptionEntity.Dosage = prescriptionModel.Dosage;
            }

            Save();
        }

        public ConsultationResultModel GetAllConsultations(int patientId)
        {
            var consultationResult = new ConsultationResultModel();

            var tempResult = _patientsContext.Consultation
                                     .Include("Prescriptions")
                                     .Where(c => c.PatientId == patientId)
                                     .OrderByDescending(c => c.ConsultationDate)
                                     .ToList();
            var result = tempResult.Select(c => new ConsultationModel
            {
                PatientId = c.PatientId,
                ConsultationId = c.ConsultationId,
                Description = c.Description,
                Prescriptions = (from p in c.Prescriptions
                                 join d in _patientsContext.Drug
                                 on p.DrugId equals d.DrugId
                                 where p.ConsultationId == c.ConsultationId
                                 select new PrescriptionModel
                                 {
                                     PrescriptionId = p.PrescriptionId,
                                     DrugId = p.DrugId,
                                     DrugName = d.DrugName,
                                     MeasureUnit = d.MeasureUnit,
                                     Dosage = p.Dosage,
                                     DosageTiming = p.DosageTiming
                                 }).ToList(),
                ConsultationDate = c.ConsultationDate,
                OthersPrescription = c.OthersPrescription,
                ImageList = (from pi in _patientsContext.PrescriptionImages
                             where pi.ConsultationId == c.ConsultationId
                             select pi.PrescriptionImageName).ToList()
            }).ToList();

            consultationResult.ConsultationModel = result;
            consultationResult.PatientModel = _patientsContext.Patients
                                     .Where(p => p.PatientId == patientId)
                                     .Select(p => new PatientModel
                                     {
                                         PatientId = p.PatientId,
                                         PatientName = p.PatientName,
                                         Gender = p.Gender,
                                         Age = p.Age,
                                         Email = p.Email,
                                         Mobile = p.Mobile,
                                         RegistrationNumber = p.RegistrationNumber,
                                         RegisteredDate = p.RegisteredDate,
                                         City = p.Address.City
                                     }).FirstOrDefault();

            return consultationResult;
        }

        public ConsultationModel GetConsultation(int consultationId)
        {
            var tempResult = _patientsContext.Consultation.Include("Prescriptions").Where(c => c.ConsultationId == consultationId).ToList();
            var result = tempResult.Select(c => new ConsultationModel
            {
                PatientId = c.PatientId,
                ConsultationId = c.ConsultationId,
                Description = c.Description,
                Prescriptions = (from p in c.Prescriptions
                                 join d in _patientsContext.Drug
                                 on p.DrugId equals d.DrugId
                                 where p.ConsultationId == c.ConsultationId
                                 select new PrescriptionModel
                                 {
                                     PrescriptionId = p.PrescriptionId,
                                     DrugId = p.DrugId,
                                     DrugName = d.DrugName,
                                     Dosage = p.Dosage,
                                     MeasureUnit = d.MeasureUnit,
                                     DosageTiming = p.DosageTiming
                                 }).ToList(),
                ConsultationDate = c.ConsultationDate,
                OthersPrescription = c.OthersPrescription,
                ImageList = (from pi in _patientsContext.PrescriptionImages
                             where pi.ConsultationId == c.ConsultationId
                             select pi.PrescriptionImageName).ToList()
            }).FirstOrDefault();

            if (result != null)
            {
                result.Patient = _patientsContext.Patients
                    .Where(p => p.PatientId == result.PatientId)
                    .Select(p => new PatientModel
                    {
                        PatientId = p.PatientId,
                        PatientName = p.PatientName,
                        Gender = p.Gender,
                        Age = p.Age,
                        Email = p.Email,
                        Mobile = p.Mobile,
                        RegistrationNumber = p.RegistrationNumber
                    }).FirstOrDefault();

                return result;
            }

            return null;
        }

        public int GetLatestConsultation(int patientId)
        {
            var i = 0;

            if (_patientsContext.Consultation.Any(c => c.PatientId == patientId))
                i = _patientsContext.Consultation.Where(c => c.PatientId == patientId).Max(c => c.ConsultationId);

            return i;
        }

        public void DeleteConsultation(int consultationId)
        {
            var prescriptions = _patientsContext.Prescription.Where(p => p.ConsultationId == consultationId).ToList();
            foreach (var prescription in prescriptions)
            {
                _patientsContext.Prescription.Remove(prescription);
            }

            var prescriptionsImages = _patientsContext.PrescriptionImages.Where(p => p.ConsultationId == consultationId).ToList();
            foreach (var prescriptionImage in prescriptionsImages)
            {
                _patientsContext.PrescriptionImages.Remove(prescriptionImage);
            }

            var consultation = _patientsContext.Consultation.FirstOrDefault(c => c.ConsultationId == consultationId);
            if (consultation != null)
            {
                _patientsContext.Consultation.Remove(consultation);
                _patientsContext.SaveChanges();
            }
        }

        private void Save()
        {
            _patientsContext.SaveChanges();
        }
    }
}
