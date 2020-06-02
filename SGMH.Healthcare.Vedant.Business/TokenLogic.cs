using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System;
using System.Security.Claims;
using System.Text;

namespace SGMH.Healthcare.Vedant.Business
{
    public class TokenLogic : ITokenLogic
    {
        public IConfiguration Configuration { get; }

        private readonly ICentreLogic _centreLogic;

        public TokenLogic(IConfiguration configuration, ICentreLogic centreLogic)
        {
            Configuration = configuration;
            _centreLogic = centreLogic;
        }

        public string Generate(AccountModel accountModel)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Security:Tokens:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, accountModel.UserName),
                    new Claim("role_id", accountModel.Role),
                    new Claim("centre_id", accountModel.CentreId.ToString()),
                    new Claim("city", GetCentre(accountModel.CentreId))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = Configuration["Security:Tokens:Audience"],
                Issuer = Configuration["Security:Tokens:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GetCentre(int? centreId) => centreId == null
            ? string.Empty
            : _centreLogic.GetCentre(Convert.ToInt32(centreId))?.City;
    }
}
