using findexit_test.Models.Dto;

namespace findexit_test.Services.Interfaces;

public interface IUserService
{
    Task<UserInfoDto?> GetUserMapped(int userId);
    Task<List<UserInfoDto>> GetAllUsersMapped();
    Task<UserInfoDto> CreateUser(UserRegisterDto request);
    Task<UserInfoDto?> UpdateUser(int userId, UpdateUserDto request);
    Task<bool> DeleteUser(int userId);
}
