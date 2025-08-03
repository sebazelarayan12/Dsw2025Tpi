using Dsw2025Tpi.Application.Dtos;
using System;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderValidator
    {
        public static void Validate(OrderModel.RequestOrderModel request)
        {
            if (request == null)
                throw new InvalidOperationException("The order cannot be void.");

            if (request.CustomerId == Guid.Empty)
                throw new InvalidOperationException("The client is mandatory.");

            if (string.IsNullOrWhiteSpace(request.ShippingAddress) || request.ShippingAddress.Length > 256)
                throw new InvalidOperationException("The shipping address is required and cannot exceed 256 characters.");

            if (string.IsNullOrWhiteSpace(request.BillingAddress) || request.BillingAddress.Length > 256)
                throw new InvalidOperationException("The billing address is required and cannot exceed 256 characters.");

            if (request.Items == null || request.Items.Count == 0)
                throw new InvalidOperationException("You must include at least one item in the order.");
        }
    }
}
