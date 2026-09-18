using findexit_test.Data;
using findexit_test.Models;
using findexit_test.Models.Dto;
using findexit_test.Repositories.Interfaces;
using findexit_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace findexit_test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private const string UserCacheKeyPrefix = "User_";
    private const string AllUsersCacheKey = "AllUsers";
    private static string GetUserCacheKey(int id) => $"{UserCacheKeyPrefix}{id}";
    private static string GetUserByEmailCacheKey(string login) => $"{UserCacheKeyPrefix}Login_{login}";


    public UserRepository(AppDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }


    public async Task<List<User>> GetAllUsers()
    {
        var cachedUsers = _cacheService.Get<List<User>>(AllUsersCacheKey);
        if (cachedUsers != null) return cachedUsers;

        var users = await _context.Users.ToListAsync();
        _cacheService.Set(AllUsersCacheKey, users);

        return users;
    }

    public async Task<User?> GetUserById(int id)
    {
        var cacheKey = GetUserCacheKey(id);
        var cachedUser = _cacheService.Get<User>(cacheKey);

        if (cachedUser != null) return cachedUser;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            _cacheService.Set(cacheKey, user);
        }

        return user;
    }

    public async Task<User?> GetUserByLogin(string login)
    {
        var cacheKey = GetUserByEmailCacheKey(login);
        var cachedUser = _cacheService.Get<User>(cacheKey);

        if (cachedUser != null) return cachedUser;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user != null)
        {
            _cacheService.Set(cacheKey, user);
            _cacheService.Set(GetUserCacheKey(user.Id), user);
        }

        return user;
    }

    public async Task<User> CreateUser(UserRegisterDto request)
    {
        var user = new User
        {
            FIO = request.FIO,
            Position = request.Position,
            Login = request.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _cacheService.Remove(AllUsersCacheKey);

        return user;
    }

    public async Task<User?> UpdateUser(int userId, UpdateUserDto updateUserDto)
    {
        var updatedRows = await _context.Users
            .Where(u => userId == u.Id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(u => u.FIO, updateUserDto.FIO)
                    .SetProperty(u => u.Position, updateUserDto.Position)
            );

        if (updatedRows != 1) return null;
        _cacheService.Remove(GetUserCacheKey(userId));
        _cacheService.Remove(AllUsersCacheKey);

        return await GetUserById(userId);
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var deletedRows = await _context.Users
            .Where(user => user.Id == userId)
            .ExecuteDeleteAsync();

        if (deletedRows != 1) return false;

        _cacheService.Remove(GetUserCacheKey(userId));
        _cacheService.Remove(AllUsersCacheKey);
        return true;
    }
}
