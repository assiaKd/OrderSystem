
using Inventory.Application.DTOs;

namespace Inventory.Application.services
{
    public interface IInventoryService
    {
        Task<bool> CanReserveStockAsync(IEnumerable<InventoryItemDto> items, CancellationToken cancellationToken);
        Task ReserveStockAsync(IEnumerable<InventoryItemDto> dtos, CancellationToken ct);
    }
}
