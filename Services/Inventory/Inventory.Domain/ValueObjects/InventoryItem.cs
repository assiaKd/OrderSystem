namespace Inventory.Domain.ValueObjects
{
    public record InventoryItem(Guid ProductId, int Quantity);
}
