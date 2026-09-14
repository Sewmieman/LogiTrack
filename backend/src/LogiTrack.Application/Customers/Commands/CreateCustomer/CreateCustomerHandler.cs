using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.DTOs;
using LogiTrack.Domain.Entities;
using MediatR;

namespace LogiTrack.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerHandler
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);

        await _context.SaveChangesAsync(cancellationToken);

        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Phone,
            customer.Email,
            customer.Address,
            customer.CreatedAt
        );
    }
}