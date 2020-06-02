namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IUserContext
    {
        string this[string key] { get; }

        int RoleId { get; }

        int CentreId { get; }
    }
}
