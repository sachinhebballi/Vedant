using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface ITokenLogic
    {
        string Generate(AccountModel accountModel);
    }
}
