using System.Collections.Generic;
using System.Net;

namespace SGMH.Healthcare.Vedant.API.Models
{
    public class ApiResponse
    {
        public HttpStatusCode Status { get; set; }
        public List<FieldLevelError> Errors { get; set; }
    }

    public class FieldLevelError
    {
        public string Field { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
