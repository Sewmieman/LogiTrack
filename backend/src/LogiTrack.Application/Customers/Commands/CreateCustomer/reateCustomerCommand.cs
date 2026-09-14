using MediatR;
using LogiTrack.Application.Customers.DTOs;

namespace LogiTrack.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string Address
) : IRequest<CustomerDto>;