using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Order.Application.DTOs;
using Order.Application.Services;

namespace Order.Presentation
{
    public static class OrderEndPoints
    {
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/orders").WithTags("Orders");


            group.MapGet("/confirmOrder/{id}", async (string id, [FromServices]  IOrderService orderService, CancellationToken cancellationToken) =>
            {
                await orderService.ConfirmOrder(id, cancellationToken);

                return Results.Ok();
            });

            group.MapPost("/createOrder", async ([FromBody] CreateOrderRequest request, [FromServices] IOrderService orderService, CancellationToken ct) =>
            {
                var orderId = await orderService.CreateOrderAsync(request, ct);
                return Results.Created($"/orders/{orderId}", orderId);
            });
        }
    }
}
