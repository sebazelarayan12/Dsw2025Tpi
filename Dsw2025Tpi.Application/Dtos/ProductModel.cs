using System.Text.Json.Serialization;

namespace Dsw2025Tpi.Application.Dtos
{
    public class ProductModel
    {
        public record Request(
            [property: JsonPropertyName("sku")] string Sku,
            [property: JsonPropertyName("internalCode")] string InternalCode,
            [property: JsonPropertyName("name")] string Name,
            [property: JsonPropertyName("description")] string Description,
            [property: JsonPropertyName("currentUnitPrice")] decimal CurrentUnitPrice,
            [property: JsonPropertyName("stockQuantity")] int StockQuantity
        );

        public record Response(
            [property: JsonPropertyName("productId")] Guid Id,
            [property: JsonPropertyName("sku")] string? Sku,
            [property: JsonPropertyName("internalCode")] string? InternalCode,
            [property: JsonPropertyName("name")] string? Name,
            [property: JsonPropertyName("description")] string? Description,
            [property: JsonPropertyName("currentUnitPrice")] decimal CurrentUnitPrice,
            [property: JsonPropertyName("stockQuantity")] int StockQuantity,
            [property: JsonPropertyName("isActive")] bool IsActive
        );
    }
}