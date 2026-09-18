using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using findexit_test.Data;
using findexit_test.Models;
using findexit_test.Models.Dto;
using findexit_test.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task = System.Threading.Tasks.Task;

namespace findexit_test.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthorizationRepository(AppDbContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }
    
    public async Task<User> Register(UserRegisterDto request)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);
        var newUser = new User
        {
             FIO = request.FIO,
             Login = request.Login,
             Position = request.Position,
             PasswordHash = hashedPassword,
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }
    public async Task<string?> Login(UserLoginRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password)) return null;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

        return await Task.FromResult(GenerateJwtToken(user));
    }

    public string GenerateJwtToken(User user)
    {
        var jsonKey = _configuration.GetValue<string>("ApiSettings:Secret");
        if (string.IsNullOrEmpty(jsonKey))
        {
            throw new InvalidOperationException("SecretKey is missing or empty in configuration.");
        }

        var key = Encoding.ASCII.GetBytes(jsonKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("UserId", user.Id.ToString()),
                new(ClaimTypes.Name, user.Login)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}