using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserOnBoarding.Data;
using UserOnBoarding.Dtos;
using UserOnBoarding.Responses;

namespace UserOnBoarding.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetAuthId()
        {
            var authUserId = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                authUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return Convert.ToInt32(authUserId);
        }

        public async Task<UserResourceResponse<UserResponseDto>> GetAuthUser()
        {
            UserResourceResponse<UserResponseDto> response = new UserResourceResponse<UserResponseDto>();

            try
            {
                var user = await _context.Users.FindAsync(GetAuthId());

                if (user == null)
                {
                    response.Data = null;

                    return response;
                }

                response.Data = new UserResponseDto(user);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
