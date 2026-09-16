using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Customers.Queries.GetCustomers;

public class GetCustomersHandler
    : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new CustomerDto(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.Email,
                x.Address,
                x.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}