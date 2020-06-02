using Microsoft.AspNetCore.Identity;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;
using SGMH.Healthcare.Vedant.Data.Entities;
using System;
using System.Threading.Tasks;

namespace SGMH.Healthcare.Vedant.Business
{
    public class AccountLogic : IAccountLogic
    {
        private readonly PatientsContext _patientsContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountLogic(PatientsContext patientsContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _patientsContext = patientsContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserModel> GetUsers()
        {
            var admins = _userManager.GetUsersInRoleAsync("Admin");
            var users = _userManager.GetUsersInRoleAsync("User");

            await Task.WhenAll(admins, users);
            return null;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.Phone
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result.Succeeded;
        }

        public async Task<bool> FindUserByUserName(string userName) => await _userManager.FindByNameAsync(userName) != null;

        public async Task<AccountModel> GetUserRoleAndCentre(string userName)
        {
            var accountModel = new AccountModel
            {
                UserName = userName
            };

            var userNameResult = await _userManager.FindByNameAsync(userName);
            if (userNameResult == null) return accountModel;

            var roles = await _userManager.GetRolesAsync(userNameResult);
            if (roles.Count <= 0) return accountModel;

            var role = await _roleManager.FindByNameAsync(roles[0]);
            accountModel.CentreId = userNameResult.CentreId;
            accountModel.Role = role.Id;

            return accountModel;
        }

        public async Task<int> AddAddress(string city)
        {
            var address = new Address
            {
                City = city,
                ModifiedBy = 0,
                ModifiedDate = DateTime.Now.ToLocalTime()
            };

            _patientsContext.Address.Add(address);

            await _patientsContext.SaveChangesAsync();

            return address.AddressId;
        }

        public async Task<bool> Authenticate(LoginModel loginModel)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                loginModel.UserName,
                loginModel.Password,
                false,
                lockoutOnFailure: false);

            return signInResult.Succeeded;
        }

        public async Task SignOut() => await _signInManager.SignOutAsync();
    }
}
