using Asp.Versioning;
using LogiTrack.Application.Auth.DTOs;
using LogiTrack.Infrastructure.Authentication;
using LogiTrack.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<LogiTrackUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly SignInManager<LogiTrackUser> _signInManager;

    public AuthController(
        UserManager<LogiTrackUser> userManager,
        IJwtTokenService jwtTokenService,
        SignInManager<LogiTrackUser> signInManager)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return BadRequest(new
            {
                message = "Email is already registered."
            });
        }

        var user = new LogiTrackUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(
            user,
            "Customer");

        var token =
            await _jwtTokenService.CreateTokenAsync(user);

        var roles =
            await _userManager.GetRolesAsync(user);

        return Ok(new AuthResponseDto(
            token,
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            roles
        ));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var result =
            await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var token =
            await _jwtTokenService.CreateTokenAsync(user);

        var roles =
            await _userManager.GetRolesAsync(user);

        return Ok(new AuthResponseDto(
            token,
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            roles
        ));
    }
}