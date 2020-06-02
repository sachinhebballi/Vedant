using System.Collections.Generic;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IDrugLogic
    {
        void Save(DrugModel drugModel);

        PagedResult<DrugModel> GetAllDrugs(int pageNumber, int pageSize);

        DrugModel GetDrug(int DrugId);

        PagedResult<DrugModel> SearchDrugs(string q, int pageNumber, int pageSize);

        void DeleteDrug(int DrugId);

        IEnumerable<DrugCategoryModel> GetDrugCategories();
    }
}
