using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Infrastructure.Data;
using OrderEntity = Order.Domain.Entities.Order;
namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderEntity?> GetOrderByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var orderId))
            {
                return null; 
            }

            return await _context.Orders
                .FirstOrDefaultAsync(a => a.Id == orderId, cancellationToken);
        }

        public async  Task SaveOrderAsync(OrderEntity order , CancellationToken cancellationToken)
        {
            var existingOrder = await _context.Orders
       .Include(o => o.Items) 
       .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

            if (existingOrder is null)
            {
                await _context.Orders.AddAsync(order, cancellationToken);
            }
            else
            {
                _context.Orders.Update(order);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
