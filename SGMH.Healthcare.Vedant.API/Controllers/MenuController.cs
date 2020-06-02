using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/Menu")]
    public class MenuController : ControllerBase
    {
        private readonly IUserContext _userContext;
        public MenuController(IUserContext userContext)
        {
            this._userContext = userContext;
        }

        [HttpGet]
        public List<MenuModel> GetMenus()
        {
            var menus = new List<MenuModel> {
                new MenuModel
                {
                    Text = "Patients",
                    Link = "/Patients",
                    Icon = "group"
                },
                new MenuModel
                {
                    Text = "Drugs",
                    Link = "/Drugs",
                    Icon = "healing"
                }
            };

            if (_userContext.RoleId == 1)
            {
                menus.Add(new MenuModel
                {
                    Text = "Centres",
                    Link = "/Centres",
                    Icon = "location_city"
                });
                menus.Add(new MenuModel
                {
                    Text = "Reports",
                    Link = "/Reports",
                    Icon = "bar_chart"
                });
            }

            return menus;
        }
    }
}