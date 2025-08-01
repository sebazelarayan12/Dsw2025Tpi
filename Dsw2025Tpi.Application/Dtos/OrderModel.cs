using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dsw2025Tpi.Application.Dtos
{
    public class OrderModel
    {
        public record OrderItemRequest(
            [property: JsonPropertyName("productId")] Guid ProductId,
            [property: JsonPropertyName("quantity")] int Quantity,
            [property: JsonPropertyName("name")] string Name,
            [property: JsonPropertyName("description")] string Description,
            [property: JsonPropertyName("unitPrice")] decimal CurrentUnitPrice
        );

        public record Request(
            [property: JsonPropertyName("customerId")] Guid CustomerId,
            [property: JsonPropertyName("shippingAddress")] string ShippingAddress,
            [property: JsonPropertyName("billingAddress")] string BillingAddress,
            [property: JsonPropertyName("orderItems")] List<OrderItemRequest> Items
        );

        public record OrderItemResponse(
            [property: JsonPropertyName("productId")] Guid ProductId,
            [property: JsonPropertyName("name")] string Name,
            [property: JsonPropertyName("description")] string Description,
            [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
            [property: JsonPropertyName("quantity")] int Quantity,
            [property: JsonPropertyName("subtotal")] decimal Subtotal
        );

        public record Response(
            [property: JsonPropertyName("orderId")] Guid Id,
            [property: JsonPropertyName("customerId")] Guid CustomerId,
            [property: JsonPropertyName("shippingAddress")] string ShippingAddress,
            [property: JsonPropertyName("billingAddress")] string BillingAddress,
            [property: JsonPropertyName("orderDate")] DateTime Date,
            [property: JsonPropertyName("totalAmount")] decimal TotalAmount,
            [property: JsonPropertyName("orderItems")] List<OrderItemResponse> Items,
            [property: JsonPropertyName("orderStatus")] string Status
        );
    }
}