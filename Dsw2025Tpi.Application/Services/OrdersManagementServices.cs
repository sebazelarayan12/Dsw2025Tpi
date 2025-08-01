using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System.Linq;

public class OrdersManagementService
{
    private readonly IRepository _repository;

    public OrdersManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderModel.Response?> GetOrderById(Guid id)
    {
        var order = await _repository.GetById<Order>(id, "Items", "Items.Product", "Customer");
        if (order == null)
            return null;

        return new OrderModel.Response(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.Date,
            order.TotalAmount,
            order.Items.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.UnitPrice,
                oi.Quantity,
                oi.Subtotal
            )).ToList(),
            order.Status.ToString()
        );
    }

    public async Task<OrderModel.Response> CreateOrder(OrderModel.Request request)
    {
        if (request == null || request.Items == null || !request.Items.Any())
            throw new ArgumentException("La orden debe tener al menos un producto.");

        var customer = await _repository.GetById<Customer>(request.CustomerId);
        if (customer == null)
            throw new ArgumentException("Cliente no encontrado.");

        // Obtener todos los productos una sola vez
        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var productsDict = new Dictionary<Guid, Product>();

        foreach (var productId in productIds)
        {
            var product = await _repository.GetById<Product>(productId);
            if (product == null)
                throw new ArgumentException($"Producto con ID {productId} no encontrado.");
            productsDict[productId] = product;
        }

        // Verificar stock ANTES de crear la orden
        foreach (var item in request.Items)
        {
            var product = productsDict[item.ProductId];
            if (item.Quantity > product.StockQuantity)
                throw new ArgumentException($"Stock insuficiente para el producto '{product.Name}'. Stock disponible: {product.StockQuantity}, solicitado: {item.Quantity}.");
        }

        // Crear la orden
        var order = new Order
        {
            Date = DateTime.UtcNow,
            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
            Notes = "",
            Status = OrderStatus.Pending,
            CustomerId = customer.Id,
            Customer = customer,
            Items = new List<OrderItem>()
        };

        decimal total = 0;

        foreach (var item in request.Items)
        {
            var product = productsDict[item.ProductId];

            var unitPrice = product.CurrentUnitPrice;  // Precio al momento de la compra
            var subtotal = unitPrice * item.Quantity;  // Subtotal por item

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
                Subtotal = subtotal
            };
            order.Items.Add(orderItem);
            total += subtotal;  // Total acumulado
        }

        // Descontar stock DESPUÉS de validar
        foreach (var item in request.Items)
        {
            var product = productsDict[item.ProductId];
            product.StockQuantity -= item.Quantity;
            await _repository.Update(product);
        }

        order.TotalAmount = total;  // Asignación final
        var added = await _repository.Add(order);

        return new OrderModel.Response(
            added.Id,
            added.CustomerId,
            added.ShippingAddress,
            added.BillingAddress,
            added.Date,
            added.TotalAmount,
            added.Items.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.UnitPrice,
                oi.Quantity,
                oi.Subtotal
            )).ToList(),
            added.Status.ToString()
        );
    }
}