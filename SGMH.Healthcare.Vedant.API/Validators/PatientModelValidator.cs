using FluentValidation;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.API.Validators
{
    public class PatientModelValidator : AbstractValidator<PatientModel>
    {
        public PatientModelValidator()
        {
            RuleFor(x => x.PatientName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.Mobile).Length(10).Matches(CustomValidator.NumbersWithoutSpaces).When(x => x.Mobile?.Length > 0);
            RuleFor(x => x.Email).EmailAddress().When(x => x.Email?.Length > 0);
            RuleFor(x => x.City).MaximumLength(50);
            RuleFor(x => x.RegistrationNumber).Length(7);
        }
    }
}
