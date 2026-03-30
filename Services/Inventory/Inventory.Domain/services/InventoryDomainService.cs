using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.services
{
    public class InventoryDomainService
    {
        public bool CanReserveStock(IEnumerable<ProductStock> stocks, IEnumerable<InventoryItem> items)
        {
            foreach (var item in items)
            {
                var stock = stocks.FirstOrDefault(s => s.ProductId == item.ProductId);
                if (stock == null || !stock.HasEnough(item.Quantity))
                    return false;
            }
            return true;
        }

        public void ReserveStock(IEnumerable<ProductStock> stocks, IEnumerable<InventoryItem> items)
        {
            foreach (var item in items)
            {
                var stock = stocks.First(s => s.ProductId == item.ProductId);
                stock.Reserve(item.Quantity);
            }
        }
    }
}
