using findexit_test.Models;
using findexit_test.Models.Dto;

namespace findexit_test.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByLogin(string login);
    Task<User> CreateUser(UserRegisterDto request);
    Task<User?> UpdateUser(int userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUser(int userId);
}
