using System.ComponentModel.DataAnnotations;

namespace UserOnBoarding.Requests
{
    public class UserRegisterRequest
    {
        [Required,MinLength(3, ErrorMessage = "Please enter at least 6 characters")]
        public string Name { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
