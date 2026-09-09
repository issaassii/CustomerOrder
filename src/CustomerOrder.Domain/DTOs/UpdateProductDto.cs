namespace CustomerOrder.Domain.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}