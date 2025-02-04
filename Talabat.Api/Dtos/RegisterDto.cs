using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string TelephoneNo { get; set; }
    }
}