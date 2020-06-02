using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class AccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public int? CentreId { get; set; }
        public string Token { get; set; }
    }
}
