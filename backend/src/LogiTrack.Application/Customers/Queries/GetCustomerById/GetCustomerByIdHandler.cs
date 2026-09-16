using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdHandler
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto?> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new CustomerDto(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.Email,
                x.Address,
                x.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}