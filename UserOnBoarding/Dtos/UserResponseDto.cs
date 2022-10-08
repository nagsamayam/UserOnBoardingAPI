using UserOnBoarding.Models;

namespace UserOnBoarding.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserResponseDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
    }
}
