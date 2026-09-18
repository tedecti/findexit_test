using findexit_test.Middlewares;
using findexit_test.Models;
using findexit_test.Models.Dto;
using findexit_test.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace findexit_test.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(IAuthorizationRepository authorizationRepository,
        IAuthorizationService authorizationService)
    {
        _authorizationRepository = authorizationRepository;
        _authorizationService = authorizationService;
    }

    [HttpPost]
    [Route("signup")]
    public async Task<ApiResponse<User>> Register([FromBody] UserRegisterDto request)
    {
        var newUser = await _authorizationRepository.Register(request);
        return new ApiResponse<User> { Message = "Success", Data = newUser };
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ApiResponse<object>> Login([FromBody] UserLoginRequestDto request)
    {
        var token = await _authorizationRepository.Login(request);
        if (token == null) throw new NullReferenceException("User not found");
        return new ApiResponse<object>() { Message = "", Data = new { token } };
    }
}