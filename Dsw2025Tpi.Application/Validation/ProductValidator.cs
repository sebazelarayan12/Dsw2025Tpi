using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;

namespace Dsw2025Tpi.Application.Validation
{
    public static class ProductValidator
    {
        public static void Validate(ProductModel.RequestProductModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new BadRequestException("SKU is required.");

            if (string.IsNullOrWhiteSpace(request.InternalCode))
                throw new BadRequestException("The internal code is mandatory.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException("Name is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new BadRequestException("Description is required.");

            if (request.CurrentUnitPrice <= 0)
                throw new BadRequestException("The price must be a positive value.");

            if (request.StockQuantity < 0)
                throw new BadRequestException("The stock quantity must be greater than or equal to 0");
        }
    }

}