using System.Threading.Tasks;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface ICentreLogic
    {
        int Save(CentreModel centreModel);

        PagedResult<CentreModel> GetAllCentres(int pageNumber, int pageSize);

        CentreModel GetCentre(int CentreId);

        PagedResult<CentreModel> SearchCentres(string q, int pageNumber, int pageSize);

        void DeleteCentre(int CentreId);

        Task<bool> CreateAccount(int centreId, string userName, string password, int addressId);
    }
}
