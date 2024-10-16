using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Models;

public class UserService {
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UserService(AppDbContext context, IConfiguration config) {
        _context = context;
        _config = config;
    }

    public string Register(User user, string password) {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Add(user);
        _context.SaveChanges();
        return GenerateJwtToken(user);
    }

    public string Login(string username, string password) {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user) {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
