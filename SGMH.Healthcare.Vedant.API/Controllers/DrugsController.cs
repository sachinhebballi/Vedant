using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMH.Healthcare.Vedant.API.Extensions;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/drugs")]
    public class DrugsController : ControllerBase
    {
        private readonly IDrugLogic _drugLogic;
        private readonly IValidator<DrugModel> _drugValidator;

        public DrugsController(IDrugLogic drugLogic, IValidator<DrugModel> drugValidator)
        {
            this._drugLogic = drugLogic;
            this._drugValidator = drugValidator;
        }

        [HttpPost]
        public IActionResult Save([FromBody]DrugModel drugModel)
        {
            var validationResult = this._drugValidator.Validate(drugModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            _drugLogic.Save(drugModel);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody]DrugModel drugModel)
        {
            var validationResult = this._drugValidator.Validate(drugModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            _drugLogic.Save(drugModel);

            return Ok();
        }

        [HttpGet]
        public PagedResult<DrugModel> GetAllDrugs(int pageNumber, int pageSize) =>
            _drugLogic.GetAllDrugs(pageNumber, pageSize);

        [HttpGet("{drugId}")]
        public DrugModel GetDrug(int drugId) => drugId == 0 ? null : _drugLogic.GetDrug(drugId);

        [HttpGet("Search")]
        public PagedResult<DrugModel> SearchDrug(string q, int pageNumber, int pageSize) =>
            string.IsNullOrWhiteSpace(q) ? null : _drugLogic.SearchDrugs(q, pageNumber, pageSize);

        [HttpDelete("{drugId}")]
        public IActionResult DeleteDrug(int drugId)
        {
            if (drugId == 0)
            {
                return BadRequest();
            }

            _drugLogic.DeleteDrug(drugId);

            return Ok();
        }

        [HttpGet("Categories")]
        public IEnumerable<DrugCategoryModel> GetDrugCategories() => _drugLogic.GetDrugCategories();
    }
}