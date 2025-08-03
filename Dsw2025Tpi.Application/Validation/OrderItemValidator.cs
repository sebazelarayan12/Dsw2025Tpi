using System;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderItemValidator
    {
        public static void Validate(OrderItemModel.RequestOrderItemModel item)
        {
            if (item == null)
                throw new InvalidOperationException("The order item cannot be null.");

            if (item.ProductId == Guid.Empty)
                throw new InvalidOperationException("The product is mandatory.");

            if (item.Quantity <= 0)
                throw new InvalidOperationException("The quantity must be greater than zero.");
        }
    }
}
