using System.Data;
using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Interfaces;
using CustomerOrder.Persistence.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Persistence.Repositories;

public class OrderReportsRepository : IOrderReportsRepository
{
    private readonly AppDbContext _context;

    public OrderReportsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerOrderSummaryDto?> GetCustomerOrderSummaryAsync(int customerId)
    {
        var connection = _context.Database.GetDbConnection();

        var sql = "SELECT * FROM \"GetCustomerOrderSummary\"(@CustomerId)";

        var result = await connection.QueryFirstOrDefaultAsync<CustomerOrderSummaryDto>(
            sql, new { CustomerId = customerId });

        return result;
    }

    public async Task<IEnumerable<OrderResponseDto>> SearchOrdersAsync(int? customerId, DateTime? startDate, DateTime? endDate)
    {
        var connection = _context.Database.GetDbConnection();

        var sql = "SELECT * FROM \"SearchOrders\"(@CustomerId, @StartDate, @EndDate)";

        var results = await connection.QueryAsync<OrderResponseDto>(
            sql, new { CustomerId = customerId, StartDate = startDate, EndDate = endDate });

        return results;
    }
}