using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        [MaxLength(20)]
        public string ConfirmPassword { get; set; }
    }
}
