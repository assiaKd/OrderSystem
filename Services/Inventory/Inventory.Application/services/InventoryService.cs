using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Domain.services;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.services
{
    public class InventoryService: IInventoryService
    {
        private readonly IProductStockRepository _repository;
        private readonly InventoryDomainService _domainService;

        public InventoryService(IProductStockRepository repository, InventoryDomainService domainService)
        {
            _repository = repository;
            _domainService = domainService;
        }

        public async Task<bool> CanReserveStockAsync(IEnumerable<InventoryItemDto> items, CancellationToken cancellationToken = default)
        {
            var inventoryItems = items
               .Select(d => new InventoryItem(d.ProductId, d.Quantity))
               .ToList();

            var productIds = items.Select(i => i.ProductId);
            var stocks = await _repository.GetByProductIdsAsync(productIds, cancellationToken);

            return _domainService.CanReserveStock(stocks, inventoryItems);
        }

        public async Task ReserveStockAsync(IEnumerable<InventoryItemDto> dtos, CancellationToken ct)
        {
            var items = dtos
                .Select(d => new InventoryItem(d.ProductId, d.Quantity))
                .ToList();

            var productIds = items.Select(i => i.ProductId);
            var stocks = await _repository.GetByProductIdsAsync(productIds, ct);

            if (!_domainService.CanReserveStock(stocks, items))
                throw new InvalidOperationException("Cannot reserve stock");

            _domainService.ReserveStock(stocks, items);

            foreach (var stock in stocks)
            {
                await _repository.SaveAsync(stock, ct);
            }
        }
    }
}
