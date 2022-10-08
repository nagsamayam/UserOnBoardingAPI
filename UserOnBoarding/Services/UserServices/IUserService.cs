using UserOnBoarding.Dtos;
using UserOnBoarding.Responses;

namespace UserOnBoarding.Services.UserServices
{
    public interface IUserService
    {
        int GetAuthId();
        Task<UserResourceResponse<UserResponseDto>> GetAuthUser();

        Task<UserCollectionResponse<User>> GetAllUsers(int page);
    }
}
