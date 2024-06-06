using AuthService.DTOs;
using AuthService.EndpointHelpers;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthDbContext _authDbContext;
    private readonly IMapper _mapper;

    public AuthenticationController(AuthDbContext authDbContext, IMapper mapper)
    {
        _authDbContext = authDbContext;
        _mapper = mapper;
    }
    [HttpPost]
    [Route("sign-up")]
    public async Task<ActionResult<SessionKeyResponseDto>> SignUp(SignUpDto signUpDto)
    {
        bool userExists = await _authDbContext.Users.AnyAsync(u => u.Email == signUpDto.Email);
        if (userExists) return Conflict("An account with this email address already exists");

        signUpDto.Password = PasswordHasher.HashPassword(signUpDto.Password);
        var user = _mapper.Map<User>(signUpDto);
        _authDbContext.Users.Add(user);
        bool succesful = await _authDbContext.SaveChangesAsync() > 0;
        if (!succesful) return StatusCode(500, "Database operation failed");

        var session = SessionGenerator.CreateSession(user.Id);
        _authDbContext.Sessions.Add(session);
        succesful = await _authDbContext.SaveChangesAsync() > 0;
        if (!succesful) return StatusCode(500, "User created. Session failed to create due to databse operation failure");
        
        return _mapper.Map<SessionKeyResponseDto>(session);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<SessionKeyResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _authDbContext.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);
        if (user == null) return Unauthorized("Invalid login credentials");

        if (!PasswordHasher.PasswordsMatch(loginDto.Password, user.HashedPassword)) return Unauthorized("Invalid login credentials");

        var existing_session = await _authDbContext.Sessions
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (existing_session != null)
        {
            _authDbContext.Sessions.Remove(existing_session);
            await _authDbContext.SaveChangesAsync();
        }
        var session = SessionGenerator.CreateSession(user.Id);
        _authDbContext.Sessions.Add(session);
        bool succesful = await _authDbContext.SaveChangesAsync() > 0;
        if (!succesful) return StatusCode(500, "Database operation failed");
        
        return _mapper.Map<SessionKeyResponseDto>(session);
    }

    [HttpPost]
    [Route("decode-session")]
    public async Task<ActionResult<SessionInfoResponseDto>> DecodeSession(DecodeSessionDto decodeSessionDto)
    {
        var session = await _authDbContext.Sessions
            .FirstOrDefaultAsync(x => x.SessionKey == decodeSessionDto.SessionKey);
        
        if (session == null) return Unauthorized();

        if (session.CreatedAt.AddMinutes(60) < DateTime.UtcNow)
        {
            _authDbContext.Sessions.Remove(session);
            await _authDbContext.SaveChangesAsync();
            return Unauthorized();
        }

        return _mapper.Map<SessionInfoResponseDto>(session);
    }
}
