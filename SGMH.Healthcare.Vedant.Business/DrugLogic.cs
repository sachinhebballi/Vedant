using System;
using System.Collections.Generic;
using System.Linq;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;

namespace SGMH.Healthcare.Vedant.Business
{
    public class DrugLogic : IDrugLogic
    {
        private readonly PatientsContext _patientsContext;

        public DrugLogic(PatientsContext patientsContext)
        {
            _patientsContext = patientsContext;
        }

        public void Save(DrugModel drugModel)
        {
            if (drugModel.DrugId == 0)
            {
                var drugEntity = new Drug
                {
                    DrugCategoryId = drugModel.CategoryId,
                    DrugName = drugModel.DrugName,
                    MeasureUnit = drugModel.MeasureUnit,
                    ModifiedDate = DateTime.Now.ToLocalTime()
                };

                _patientsContext.Drug.Add(drugEntity);
            }
            else
            {
                var drugEntity = _patientsContext.Drug.FirstOrDefault(c => c.DrugId == drugModel.DrugId);
                if (drugEntity != null)
                {
                    drugEntity.DrugCategoryId = drugModel.CategoryId;
                    drugEntity.DrugName = drugModel.DrugName;
                    drugEntity.MeasureUnit = drugModel.MeasureUnit;
                    drugEntity.ModifiedDate = DateTime.Now.ToLocalTime();
                }
            }

            Save();
        }

        public PagedResult<DrugModel> GetAllDrugs(int pageNumber, int pageSize)
        {
            var drugs = from d in _patientsContext.Drug
                           join dc in _patientsContext.DrugCategory
                           on d.DrugCategoryId equals dc.DrugCategoryId
                           orderby dc.DrugCategoryName, d.DrugName

                           select new DrugModel
                           {
                               DrugId = d.DrugId,
                               CategoryName = dc.DrugCategoryName,
                               DrugName = d.DrugName,
                               MeasureUnit = d.MeasureUnit
                           };

            return Paginate(pageNumber, pageSize, drugs);
        }

        public DrugModel GetDrug(int drugId)
        {
            var drug = from d in _patientsContext.Drug
                       join dc in _patientsContext.DrugCategory
                       on d.DrugCategoryId equals dc.DrugCategoryId
                       where d.DrugId == drugId
                       select new DrugModel
                       {
                           DrugId = d.DrugId,
                           DrugName = d.DrugName,
                           MeasureUnit = d.MeasureUnit,
                           CategoryId = dc.DrugCategoryId,
                           CategoryName = dc.DrugCategoryName
                       };

            return drug.FirstOrDefault();
        }

        public PagedResult<DrugModel> SearchDrugs(string q, int pageNumber, int pageSize)
        {
            var drugs = from d in _patientsContext.Drug
                           join dc in _patientsContext.DrugCategory
                           on d.DrugCategoryId equals dc.DrugCategoryId
                           where d.DrugName.Contains(q)
                           select new DrugModel
                           {
                               DrugId = d.DrugId,
                               DrugName = d.DrugName,
                               CategoryId = dc.DrugCategoryId,
                               MeasureUnit = d.MeasureUnit,
                               CategoryName = dc.DrugCategoryName
                           };

            return Paginate(pageNumber, pageSize, drugs);
        }

        public void DeleteDrug(int drugId)
        {
            var drug = _patientsContext.Drug.FirstOrDefault(c => c.DrugId == drugId);
            if (drug != null)
            {
                _patientsContext.Drug.Remove(drug);
                _patientsContext.SaveChanges();
            }
        }

        public IEnumerable<DrugCategoryModel> GetDrugCategories()
        {
            var drugCategories = from d in _patientsContext.DrugCategory
                                 select new DrugCategoryModel
                                 {
                                     CategoryId = d.DrugCategoryId,
                                     CategoryName = d.DrugCategoryName
                                 };

            return drugCategories.ToList();
        }

        private PagedResult<DrugModel> Paginate(int pageNumber, int pageSize, IQueryable<DrugModel> patients)
        {
            return new PagedResult<DrugModel>
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
