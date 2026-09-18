using findexit_test.Models;
using findexit_test.Models.Dto;

namespace findexit_test.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<User> Register(UserRegisterDto request);
    Task<string?> Login(UserLoginRequestDto request);
    string GenerateJwtToken(User user);
}