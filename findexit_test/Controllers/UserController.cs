using AutoMapper;
using findexit_test.Middlewares;
using findexit_test.Models.Dto;
using findexit_test.Repositories.Interfaces;
using findexit_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace findexit_test.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UserController(IUserRepository userRepository,  IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ApiResponse<object>> GetAllUsers()
    {
        var result = await _userService.GetAllUsersMapped();
        return new ApiResponse<object>() { Data = result,  Message = "Success" };
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetUser(int id)
    {
        var user = await _userService.GetUserMapped(id);
        if (user == null)
            return NotFound(new ApiResponse<UserInfoDto> { Message = "User not found" });

        return Ok(new ApiResponse<UserInfoDto> { Message = "Success", Data = user });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserInfoDto>>> CreateUser(UserRegisterDto request)
    {
        var existingUser = await _userRepository.GetUserByLogin(request.Login);
        if (existingUser != null)
            return Conflict(new ApiResponse<UserInfoDto> { Message = "Login already exists" });

        var user = await _userService.CreateUser(request);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id },
            new ApiResponse<UserInfoDto> { Message = "User created", Data = user });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserInfoDto>>> UpdateUser(int id, UpdateUserDto request)
    {
        var user = await _userService.UpdateUser(id, request);
        if (user == null)
            return NotFound(new ApiResponse<UserInfoDto> { Message = "User not found" });

        return Ok(new ApiResponse<UserInfoDto> { Message = "User updated", Data = user });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int id)
    {
        var deleted = await _userService.DeleteUser(id);
        if (!deleted)
            return NotFound(new ApiResponse<object> { Message = "User not found" });

        return Ok(new ApiResponse<object> { Message = "User deleted" });
    }
}
