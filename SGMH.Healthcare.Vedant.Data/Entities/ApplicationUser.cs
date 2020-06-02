using Microsoft.AspNetCore.Identity;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public int RoleId { get; set; }
        public int AddressId { get; set; }
        public int CentreId { get; set; }
    }
}
