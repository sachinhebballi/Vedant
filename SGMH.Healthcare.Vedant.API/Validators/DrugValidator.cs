using FluentValidation;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.API.Validators
{
    public class DrugValidator : AbstractValidator<DrugModel>
    {
        public DrugValidator()
        {
            RuleFor(x => x.DrugName).NotEmpty().MaximumLength(100);
        }
    }
}
