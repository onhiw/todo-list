using Microsoft.AspNetCore.Mvc;
using TodoListApi.Models;
using TodoListApi.Services;
using TodoListApi.Dtos;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly UserService _userService;

    public AuthController(UserService userService) {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDto dto) {
        var token = _userService.Register(new User { Username = dto.Username }, dto.Password);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto dto) {
        var token = _userService.Login(dto.Username, dto.Password);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}
