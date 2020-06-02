using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
