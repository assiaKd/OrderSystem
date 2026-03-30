using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using StackExchange.Redis;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly IDatabase _redisDatabase;
        private const string RedisKeyPrefix = "inventory_";

        public ProductStockRepository(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }


        public async Task<List<ProductStock>> GetByProductIdsAsync(
           IEnumerable<Guid> productIds,
           CancellationToken cancellationToken = default)
        {
            var stocks = new List<ProductStock>();

            foreach (var productId in productIds)
            {
                var key = RedisKeyPrefix + productId;

                var value = await _redisDatabase.StringGetAsync(key);

                if (value.HasValue && value.TryParse(out int quantity))
                {
                    stocks.Add(new ProductStock(productId, quantity));
                }
                else
                {
                    stocks.Add(new ProductStock(productId, 0));
                }
            }

            return stocks;
        }

        public async Task SaveAsync(
            ProductStock stock,
            CancellationToken cancellationToken = default)
        {
            var key = RedisKeyPrefix + stock.ProductId;

            await _redisDatabase.StringSetAsync(
                key,
                stock.AvailableQuantity
            );
        }
    }
}
