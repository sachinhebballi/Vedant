using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentValidation;
using SGMH.Healthcare.Vedant.API.Extensions;
using SGMH.Healthcare.Vedant.API.Models;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountLogic _accountLogic;
        private readonly ITokenLogic _tokenLogic;
        private readonly IValidator<LoginModel> _loginValidator;

        public AccountController(IAccountLogic accountLogic, ITokenLogic tokenLogic, IValidator<LoginModel> loginValidator)
        {
            _accountLogic = accountLogic;
            _tokenLogic = tokenLogic;
            _loginValidator = loginValidator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                result = await _accountLogic.Register(registerModel);
            }

            return Ok(result);
        }

        [HttpPost("user/validate")]
        public async Task<IActionResult> ValidateUser([FromBody]LoginModel loginModel)
        {
            if (loginModel == null) return StatusCode((int)HttpStatusCode.NotFound, false);
            
            var userExists = await _accountLogic.FindUserByUserName(loginModel.UserName);
            return Ok(userExists);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            var validationResult = _loginValidator.Validate(loginModel);
            if (!validationResult.IsValid)
                return BadRequest(new ApiResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Errors = validationResult.GetFieldLevelErrors()
                });

            var isAuthenticated = await _accountLogic.Authenticate(loginModel);
            if (!isAuthenticated)
            {
                return StatusCode((int)HttpStatusCode.OK, new AccountModel { UserName = loginModel.UserName });
            }

            var accountModel = await _accountLogic.GetUserRoleAndCentre(loginModel.UserName);
            accountModel.Token = _tokenLogic.Generate(accountModel);

            return Ok(accountModel);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> SignOut()
        {
            await _accountLogic.SignOut();

            return Ok();
        }
    }
}