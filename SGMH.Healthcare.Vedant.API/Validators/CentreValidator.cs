using FluentValidation;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.API.Validators
{
    public class CentreValidator : AbstractValidator<CentreModel>
    {
        public CentreValidator()
        {
            RuleFor(x => x.CentreName).NotEmpty().MaximumLength(100);
        }
    }
}
