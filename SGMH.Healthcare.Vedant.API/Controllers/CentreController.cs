using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMH.Healthcare.Vedant.API.Extensions;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System.Threading.Tasks;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/centres")]
    public class CentreController : ControllerBase
    {
        private readonly ICentreLogic _centreLogic;
        private readonly IAccountLogic _accountLogic;
        private readonly IValidator<CentreModel> _centreValidator;

        public CentreController(ICentreLogic centreLogic, IValidator<CentreModel> centreValidator, IAccountLogic accountLogic)
        {
            this._centreLogic = centreLogic;
            this._accountLogic = accountLogic;
            this._centreValidator = centreValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody]CentreModel centreModel)
        {
            var validationResult = this._centreValidator.Validate(centreModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            if (centreModel.ImageList != null)
            {
                centreModel.Logo = centreModel.ImageList.Count > 0 ? centreModel.ImageList[0] : null;
            }

            int centreId = _centreLogic.Save(centreModel);

            var addressId = await this._accountLogic.AddAddress(centreModel.City);
            return Ok(await _centreLogic.CreateAccount(centreId, centreModel.Username, centreModel.Password, addressId));
        }

        [HttpPut]
        public IActionResult Update([FromBody]CentreModel centreModel)
        {
            var validationResult = this._centreValidator.Validate(centreModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            if (centreModel.ImageList != null)
            {
                centreModel.Logo = centreModel.ImageList.Count > 0 ? centreModel.ImageList[0] : null;
            }

            _centreLogic.Save(centreModel);

            return Ok();
        }

        [HttpGet]
        public PagedResult<CentreModel> GetAllCentres(int pageNumber, int pageSize) => _centreLogic.GetAllCentres(pageNumber, pageSize);

        [HttpGet("{centreId}")]
        public CentreModel GetCentre(int centreId) => centreId == 0 ? null : _centreLogic.GetCentre(centreId);

        [HttpGet("Search")]
        public PagedResult<CentreModel> SearchCentre(string q, int pageNumber, int pageSize) =>
            string.IsNullOrWhiteSpace(q) ? null : _centreLogic.SearchCentres(q, pageNumber, pageSize);

        [HttpDelete("{centreId}")]
        public IActionResult DeleteCentre(int centreId)
        {
            if (centreId == 0)
            {
                return BadRequest();
            }

            _centreLogic.DeleteCentre(centreId);

            return Ok();
        }
    }
}