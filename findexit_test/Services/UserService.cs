using AutoMapper;
using findexit_test.Models.Dto;
using findexit_test.Repositories.Interfaces;
using findexit_test.Services.Interfaces;

namespace findexit_test.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;


    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserInfoDto?> GetUserMapped(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) return null;
        return _mapper.Map<UserInfoDto>(user);
    }

    public async Task<List<UserInfoDto>> GetAllUsersMapped()
    {
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<List<UserInfoDto>>(users);
    }

    public async Task<UserInfoDto> CreateUser(UserRegisterDto request)
    {
        var user = await _userRepository.CreateUser(request);
        return _mapper.Map<UserInfoDto>(user);
    }

    public async Task<UserInfoDto?> UpdateUser(int userId, UpdateUserDto request)
    {
        var user = await _userRepository.UpdateUser(userId, request);
        return user == null ? null : _mapper.Map<UserInfoDto>(user);
    }

    public Task<bool> DeleteUser(int userId)
    {
        return _userRepository.DeleteUser(userId);
    }
}
