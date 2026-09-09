namespace CustomerOrder.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}