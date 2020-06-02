using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;

namespace SGMH.Healthcare.Vedant.Business
{
    public class CentreLogic : ICentreLogic
    {
        private readonly PatientsContext _patientsContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CentreLogic(PatientsContext patientsContext, UserManager<ApplicationUser> userManager)
        {
            _patientsContext = patientsContext;
            _userManager = userManager;
        }

        public int Save(CentreModel centreModel)
        {
            var centreEntity = new Centre();
            var centre = _patientsContext.Centres.FirstOrDefault(c => c.CentreName == centreModel.CentreName && c.City == centreModel.City);
            if (centreModel.CentreId == 0)
            {
                if (centre != null)
                {
                    centre.IsActive = true;
                    centre.ModifiedDate = DateTime.Now.ToLocalTime();
                }
                else
                {
                    centreEntity = new Centre
                    {
                        CentreName = centreModel.CentreName,
                        CentreCode = centreModel.CentreCode,
                        IsActive = true,
                        AddressLine1 = centreModel.AddressLine1,
                        AddressLine2 = centreModel.AddressLine2,
                        City = centreModel.City,
                        ZipCode = centreModel.ZipCode,
                        ContactNumber = centreModel.ContactNumber,
                        DoctorName = centreModel.DoctorName,
                        Logo = centreModel.Logo,
                        ModifiedDate = DateTime.Now.ToLocalTime()
                    };

                    _patientsContext.Centres.Add(centreEntity);
                }
            }
            else
            {
                centreEntity = _patientsContext.Centres.FirstOrDefault(c => c.CentreId == centreModel.CentreId);
                if (centreEntity != null)
                {
                    centreEntity.CentreName = centreModel.CentreName;
                    centreEntity.CentreCode = centreModel.CentreCode;
                    centreEntity.IsActive = centreModel.IsActive;
                    centreEntity.ModifiedDate = DateTime.Now.ToLocalTime();
                    centreEntity.AddressLine1 = centreModel.AddressLine1;
                    centreEntity.AddressLine2 = centreModel.AddressLine2;
                    centreEntity.City = centreModel.City;
                    centreEntity.ZipCode = centreModel.ZipCode;
                    centreEntity.ContactNumber = centreModel.ContactNumber;
                    centreEntity.DoctorName = centreModel.DoctorName;
                    centreEntity.Logo = centreModel.Logo;
                }
            }

            Save();

            return centreEntity?.CentreId ?? 0;
        }

        public async Task<bool> CreateAccount(int centreId, string userName, string password, int addressId = 0)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                CentreId = centreId,
                AddressId = addressId,
                RoleId = (int)Role.Doctor
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.Doctor.ToString());
            }

            return result.Succeeded;
        }

        public PagedResult<CentreModel> GetAllCentres(int pageNumber, int pageSize)
        {
            var centres = from c in _patientsContext.Centres
                          where c.IsActive
                          orderby c.CentreName
                          select new CentreModel
                          {
                              CentreId = c.CentreId,
                              CentreCode = c.CentreCode,
                              CentreName = c.CentreName,
                              ContactNumber = c.ContactNumber,
                              DoctorName = c.DoctorName,
                              IsActive = c.IsActive,
                              AddressLine1 = c.AddressLine1,
                              AddressLine2 = c.AddressLine2,
                              City = c.City,
                              ZipCode = c.ZipCode
                          };

            return Paginate(pageNumber, pageSize, centres);
        }

        public CentreModel GetCentre(int centreId)
        {
            var centre = from c in _patientsContext.Centres
                         orderby c.ModifiedDate descending
                         where c.CentreId == centreId
                         select new CentreModel
                         {
                             CentreId = c.CentreId,
                             CentreCode = c.CentreCode,
                             CentreName = c.CentreName,
                             ContactNumber = c.ContactNumber,
                             DoctorName = c.DoctorName,
                             IsActive = c.IsActive,
                             AddressLine1 = c.AddressLine1,
                             AddressLine2 = c.AddressLine2,
                             City = c.City,
                             ZipCode = c.ZipCode
                         };

            return centre.FirstOrDefault();
        }

        public PagedResult<CentreModel> SearchCentres(string q, int pageNumber, int pageSize)
        {
            var centres = from c in _patientsContext.Centres
                          orderby c.CentreName
                          where c.IsActive && c.CentreName.Contains(q)
                          select new CentreModel
                          {
                              CentreId = c.CentreId,
                              CentreCode = c.CentreCode,
                              CentreName = c.CentreName,
                              ContactNumber = c.ContactNumber,
                              DoctorName = c.DoctorName,
                              IsActive = c.IsActive,
                              AddressLine1 = c.AddressLine1,
                              AddressLine2 = c.AddressLine2,
                              City = c.City,
                              ZipCode = c.ZipCode
                          };

            return Paginate(pageNumber, pageSize, centres);
        }

        public void DeleteCentre(int centreId)
        {
            var centre = _patientsContext.Centres.FirstOrDefault(c => c.CentreId == centreId);
            if (centre == null) return;
            
            centre.IsActive = false;
            Save();
        }

        private PagedResult<CentreModel> Paginate(int pageNumber, int pageSize, IQueryable<CentreModel> patients)
        {
            return new PagedResult<CentreModel>
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
