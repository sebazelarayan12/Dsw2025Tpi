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
                throw new Exceptions.ApplicationException("SKU is required.");

            if (string.IsNullOrWhiteSpace(request.InternalCode))
                throw new Exceptions.ApplicationException("The internal code is mandatory.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exceptions.ApplicationException("Name is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new Exceptions.ApplicationException("Description is required.");

            if (request.CurrentUnitPrice <= 0)
                throw new Exceptions.ApplicationException("The price must be a positive value.");

            if (request.StockQuantity < 0)
                throw new Exceptions.ApplicationException("The stock quantity must be greater than or equal to 0");
        }
    }

}