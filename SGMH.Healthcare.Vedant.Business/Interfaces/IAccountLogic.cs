using System.Threading.Tasks;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IAccountLogic
    {
        Task<bool> Register(RegisterModel registerModel);

        Task<UserModel> GetUsers();

        Task<AccountModel> GetUserRoleAndCentre(string userName);

        Task<bool> FindUserByUserName(string userName);

        Task<bool> Authenticate(LoginModel loginModel);

        Task<int> AddAddress(string city);

        Task SignOut();
    }
}
