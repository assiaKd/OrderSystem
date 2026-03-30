
namespace Inventory.Domain.Entities
{
    public class ProductStock
    {
        public Guid ProductId { get; private set; }
        public int AvailableQuantity { get; private set; }

        public ProductStock(Guid productId, int quantity)
        {
            ProductId = productId;
            AvailableQuantity = quantity;
        }

        public bool HasEnough(int quantity)
        {
            return AvailableQuantity >= quantity;
        }

        public void Reserve(int quantity)
        {
            if (quantity > AvailableQuantity)
                throw new Exception("Not enough stock");

            AvailableQuantity -= quantity;
        }
    }
}
