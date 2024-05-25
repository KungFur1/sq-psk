using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AuthDbContext _authDbContext;
    private readonly IMapper _mapper;

    public UsersController(AuthDbContext authDbContext, IMapper mapper)
    {
        _authDbContext = authDbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<UserPublicResponseDto>> GetUserById(Guid id)
    {
        var user = await _authDbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return NotFound();

        return _mapper.Map<UserPublicResponseDto>(user);
    }
}
