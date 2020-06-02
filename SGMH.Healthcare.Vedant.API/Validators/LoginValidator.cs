using FluentValidation;
using SGMH.Healthcare.Vedant.API.Models;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithErrorCode(ApiErrorCodes.UserBlank.Code)
                .WithMessage(ApiErrorCodes.UserBlank.Message)
                .Matches(CustomValidator.Username)
                .WithErrorCode(ApiErrorCodes.UserInvalid.Code)
                .WithMessage(ApiErrorCodes.UserInvalid.Message)
                .MaximumLength(50)
                .WithErrorCode(ApiErrorCodes.UserLength.Code)
                .WithMessage(ApiErrorCodes.UserLength.Message);
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(ApiErrorCodes.PasswordBlank.Code)
                .WithMessage(ApiErrorCodes.PasswordBlank.Message)
                .MaximumLength(20)
                .WithErrorCode(ApiErrorCodes.PasswordLength.Code)
                .WithMessage(ApiErrorCodes.PasswordLength.Message);
        }
    }
}
