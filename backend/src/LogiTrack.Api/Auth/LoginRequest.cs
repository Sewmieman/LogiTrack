namespace LogiTrack.Api.Auth;

public record LoginRequest(
    string Email,
    string Password);