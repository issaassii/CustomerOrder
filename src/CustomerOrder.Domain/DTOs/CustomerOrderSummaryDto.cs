namespace CustomerOrder.Domain.DTOs;

public class CustomerOrderSummaryDto
{
    public int CustomerId { get; set; }
    public long TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}