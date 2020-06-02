using System;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string RegisteredCentre { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}