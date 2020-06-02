using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using SGMH.Healthcare.Vedant.API.Models;

namespace SGMH.Healthcare.Vedant.API.Extensions
{
    public static class FluentValidationExtensions
    {
        public static List<FieldLevelError> GetFieldLevelErrors(this ValidationResult validationResult)
        {
            var errorsList = new List<FieldLevelError>();

            var errorMessages = validationResult.Errors;
            foreach (var errorMessage in errorMessages)
            {
                errorsList.Add(new FieldLevelError
                {
                    Field = errorMessage.PropertyName,
                    Code = errorMessage.ErrorCode,
                    Message = errorMessage.ErrorMessage
                });
            }

            return errorsList;
        }

        public static List<string> GetErrorMessages(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}
