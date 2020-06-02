using System.Linq;
using Microsoft.AspNetCore.Http;
using SGMH.Healthcare.Vedant.Business.Interfaces;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class UserContext : IUserContext
    {
        private string centreId = "centre_id";
        private string roleId = "role_id";

        private readonly IHttpContextAccessor httpContext;

        public UserContext(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public string this[string key] => httpContext.HttpContext.User.Claims.SingleOrDefault(c => c.Type == key)?.Value;

        public int RoleId => string.IsNullOrWhiteSpace(this[roleId]) ? 0 : int.Parse(this[roleId]);

        public int CentreId => string.IsNullOrWhiteSpace(this[centreId]) ? 0 : int.Parse(this[centreId]);
    }
}
