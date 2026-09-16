using LogiTrack.Application.Customers.DTOs;
using MediatR;

namespace LogiTrack.Application.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(int Id)
    : IRequest<CustomerDto?>;