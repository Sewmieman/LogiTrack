namespace LogiTrack.Application.Customers.DTOs;

public record CustomerDto(
    int Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string Address,
    DateTime CreatedAt
);