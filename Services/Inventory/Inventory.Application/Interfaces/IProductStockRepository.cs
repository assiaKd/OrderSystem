

using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IProductStockRepository
    {
        Task<List<ProductStock>> GetByProductIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default);
        Task SaveAsync(ProductStock stock, CancellationToken cancellationToken = default);
    }
}
