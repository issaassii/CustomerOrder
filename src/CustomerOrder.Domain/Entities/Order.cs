using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrder.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}