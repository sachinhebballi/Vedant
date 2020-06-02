using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGMH.Healthcare.Vedant.API.Models
{
    public class ApiErrorCodes
    {
        public static readonly ApiError UserBlank = new ApiError { Code = "1000", Message = "Please provide username" };
        public static readonly ApiError UserInvalid = new ApiError { Code = "1001", Message = "Please provide a valid username" };
        public static readonly ApiError UserLength = new ApiError { Code = "1002", Message = "Username should not exceed 50 characters" };
        public static readonly ApiError PasswordBlank = new ApiError { Code = "1003", Message = "Please provide password" };
        public static readonly ApiError PasswordLength = new ApiError { Code = "1004", Message = "Password should not exceed 20 characters" };
    }

    public class ApiError
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
