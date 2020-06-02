using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SGMH.Healthcare.Vedant.Business
{
    public class PatientLogic : IPatientLogic
    {
        private readonly PatientsContext _patientsContext;
        private readonly IUserContext _userContext;

        public PatientLogic(PatientsContext patientsContext, IUserContext userContext)
        {
            _patientsContext = patientsContext;
            this._userContext = userContext;
        }

        public void Save(PatientModel patientModel)
        {
            if (patientModel.PatientId == 0)
            {
                var addressEntity = new Address
                {
                    City = patientModel.City,
                    ModifiedDate = DateTime.Now.ToLocalTime()
                };

                var patientEntity = new Patient
                {
                    PatientName = patientModel.PatientName,
                    Age = patientModel.Age,
                    Gender = patientModel.Gender,
                    Mobile = patientModel.Mobile,
                    Email = patientModel.Email,
                    Address = addressEntity,
                    IsActive = true,
                    RegisteredDate = patientModel.RegisteredDate,
                    ModifiedDate = DateTime.Now.ToLocalTime(),
                    RegistrationNumber = patientModel.RegistrationNumber,
                    CentreId = _userContext.CentreId == 0 ? patientModel.RegisteredCentreId : _userContext.CentreId
                };

                _patientsContext.Patients.Add(patientEntity);
            }
            else
            {
                var patientEntity = _patientsContext.Patients.FirstOrDefault(c => c.PatientId == patientModel.PatientId);
                if (patientEntity != null)
                {
                    patientEntity.PatientName = patientModel.PatientName;
                    patientEntity.Mobile = patientModel.Mobile;
                    patientEntity.Email = patientModel.Email;
                    patientEntity.Gender = patientModel.Gender;
                    patientEntity.Age = patientModel.Age;
                    patientEntity.ModifiedDate = DateTime.Now.ToLocalTime();

                    var customerAddressEntity =
                        _patientsContext.Address.FirstOrDefault(c => c.AddressId == patientEntity.AddressId);
                    if (customerAddressEntity != null) customerAddressEntity.City = patientModel.City;
                }
            }

            Save();
        }

        public string GetNewPatientId(int? cId)
        {
            var centreId = cId ?? _userContext.CentreId;
            var patientId = _patientsContext.Patients.Any(c => c.CentreId == centreId) ? _patientsContext.Patients.Where(c => c.CentreId == centreId).Max(c => c.PatientId) : 0;
            if (patientId > 0)
            {
                var currentRegistrationNumber = _patientsContext.Patients.Where(c => c.PatientId == patientId).Select(c => new { c.RegistrationNumber }).FirstOrDefault();
                var centreCode = currentRegistrationNumber?.RegistrationNumber.Substring(0, 3).ToUpper();

                return centreCode + (Convert.ToInt32((currentRegistrationNumber?.RegistrationNumber)?.Substring(3)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                var centreCode = _patientsContext.Centres.Where(c => c.CentreId == centreId).Select(c => c.CentreCode).FirstOrDefault();
                return centreCode + "0001";
            }
        }

        public PagedResult<PatientModel> GetAllPatients(int? centreId, int pageNumber, int pageSize = 10)
        {
            var currentCentreId = (Role)_userContext.RoleId == Role.Admin && (centreId != null && centreId != 0)
                ? Convert.ToInt32(centreId)
                : _userContext.CentreId;

            var patients = (from p in _patientsContext.Patients
                            join pt in (from ct in _patientsContext.Consultation
                                        group ct by ct.PatientId into grouping
                                        select new { PatientId = grouping.Key, Consultations = grouping.Count() })
                                        on p.PatientId equals pt.PatientId into ptc
                            from abc in ptc.DefaultIfEmpty()
                            join a in _patientsContext.Address
                                            on p.AddressId equals a.AddressId
                            join c in _patientsContext.Centres
                            on p.CentreId equals c.CentreId
                            where p.IsActive
                            orderby p.RegisteredDate descending
                            select new PatientModel
                            {
                                PatientId = p.PatientId,
                                PatientName = p.PatientName,
                                Mobile = p.Mobile,
                                City = a.City,
                                Age = p.Age,
                                Email = p.Email,
                                Gender = p.Gender,
                                RegisteredDate = p.RegisteredDate,
                                RegisteredCentreId = p.CentreId,
                                RegisteredCentre = c.City,
                                RegistrationNumber = p.RegistrationNumber,
                                Consultations = abc.Consultations.ToString()
                            });

            var role = (Role)_userContext.RoleId;
            patients = role == Role.Doctor || role == Role.Admin && currentCentreId != 0 ? patients.Where(p => p.RegisteredCentreId == currentCentreId) : patients;

            return Paginate(pageNumber, pageSize, patients);
        }

        public PatientModel GetPatient(int patientId)
        {
            var patient = from p in _patientsContext.Patients
                          join a in _patientsContext.Address
                          on p.AddressId equals a.AddressId
                          join c in _patientsContext.Centres
                          on p.CentreId equals c.CentreId
                          where p.PatientId == patientId && p.IsActive
                          select new PatientModel
                          {
                              PatientId = p.PatientId,
                              PatientName = p.PatientName,
                              Mobile = p.Mobile,
                              City = a.City,
                              Age = p.Age,
                              Email = p.Email,
                              Gender = p.Gender,
                              RegisteredDate = p.RegisteredDate,
                              RegisteredCentre = c.City,
                              RegisteredCentreId = p.CentreId,
                              RegistrationNumber = p.RegistrationNumber
                          };

            return patient.FirstOrDefault();
        }

        public PagedResult<PatientModel> SearchPatients(string q, int? centreId, int pageNumber, int pageSize)
        {
            var role = (Role)_userContext.RoleId;
            var currentCentreId = (Role)_userContext.RoleId == Role.Admin && (centreId != null && centreId != 0)
                            ? Convert.ToInt32(centreId)
                            : _userContext.CentreId;

            var patients = from p in _patientsContext.Patients
                           join a in _patientsContext.Address
                           on p.AddressId equals a.AddressId
                           join c in _patientsContext.Centres
                           on p.CentreId equals c.CentreId
                           orderby p.ModifiedDate descending
                           where p.IsActive && p.PatientName.Contains(q) || p.Mobile.Contains(q) || p.RegistrationNumber.Contains(q)
                           select new PatientModel
                           {
                               PatientId = p.PatientId,
                               PatientName = p.PatientName,
                               Mobile = p.Mobile,
                               City = a.City,
                               Age = p.Age,
                               Email = p.Email,
                               Gender = p.Gender,
                               RegisteredDate = p.RegisteredDate,
                               RegisteredCentre = c.City,
                               RegistrationNumber = p.RegistrationNumber,
                               RegisteredCentreId = p.CentreId
                           };

            patients = role == Role.Doctor || role == Role.Admin && centreId != 0 ? patients.Where(p => p.RegisteredCentreId == currentCentreId) : patients;

            return Paginate(pageNumber, pageSize, patients);
        }

        public void DeletePatient(int patientId)
        {
            var patient = _patientsContext.Patients.FirstOrDefault(p => p.PatientId == patientId);
            if (patient != null)
            {
                patient.IsActive = false;
            }

            Save();
        }

        public List<string> GetCities(string q)
        {
            return _patientsContext.Address.Where(a => a.City.Contains(q)).Select(a => a.City).Distinct().ToList();
        }

        private PagedResult<PatientModel> Paginate(int pageNumber, int pageSize, IQueryable<PatientModel> patients)
        {
            return new PagedResult<PatientModel>
            {
                Index = pageNumber,
                Count = patients.Count(),
                Result = patients.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList()
            };
        }

        private void Save()
        {
            _patientsContext.SaveChanges();
        }
    }
}
