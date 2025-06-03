namespace InventoryAPI.Application;

public class ProductMessage
{
    public string Action { get; set; }
    public Product Product { get; set; }
}