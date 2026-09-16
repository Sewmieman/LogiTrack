using LogiTrack.Application.Customers.DTOs;
using MediatR;

namespace LogiTrack.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IRequest<List<CustomerDto>>;