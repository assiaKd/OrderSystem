
using Inventory.Application.DTOs;
using Inventory.Application.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Inventory.Presentation
{
    public static class InventoryEndpoints
    {
        public static void MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/inventory").WithTags("Inventory");

            group.MapPost("/can-reserve", async (
              [FromBody] IEnumerable<InventoryItemDto> items,
               [FromServices] IInventoryService inventoryService,
                CancellationToken ct) =>
            {
                var canReserve = await inventoryService.CanReserveStockAsync(items, ct);

                return Results.Ok(new { canReserve });
            });

            group.MapPost("/reserve", async (
              [FromBody] IEnumerable<InventoryItemDto> items,
              [FromServices] IInventoryService inventoryService,
                CancellationToken ct) =>
            {
                try
                {
                    await inventoryService.ReserveStockAsync(items, ct);
                    return Results.Ok();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            });
        }
    }
}