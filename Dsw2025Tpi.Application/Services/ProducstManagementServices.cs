using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService
    {
        private readonly IRepository _repository;

        public ProductsManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductModel.ResponseProductModel?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            return product == null
                ? throw new EntityNotFoundException("Product not found")
                : new ProductModel.ResponseProductModel(product.Id, product.Sku, product.InternalCode, product.Name, product.Description, product.CurrentUnitPrice, product.StockQuantity, product.IsActive);
        }

        public async Task<IEnumerable<ProductModel.ResponseProductModel>?> GetAllProducts()
        {
            return (await _repository
                .GetFiltered<Product>(p => p.IsActive))?
                .Select(p => new ProductModel.ResponseProductModel(p.Id, p.Sku, p.InternalCode, p.Name, p.Description,
                p.CurrentUnitPrice, p.StockQuantity, p.IsActive));
        }

        public async Task<ProductModel.ResponseProductModel> AddProduct(ProductModel.RequestProductModel request)
        {
            ProductValidator.Validate(request);
            var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (exist != null) throw new DuplicatedEntityException($"Un producto con Sku {request.Sku} ya existe");
            var product = new Product(request.Sku, request.InternalCode, request.Name, request.Description, request.CurrentUnitPrice, request.StockQuantity);
            await _repository.Add(product);
            return new ProductModel.ResponseProductModel(product.Id, product.Sku, product.InternalCode, product.Name, product.Description,
                product.CurrentUnitPrice, product.StockQuantity, product.IsActive);
        }
        public async Task<ProductModel.ResponseProductModel> UpdateProduct(Guid id, ProductModel.RequestProductModel request)
        {
            var exist = await _repository.GetById<Product>(id);
            if (exist == null)
                throw new EntityNotFoundException("Product not found");

            ProductValidator.Validate(request);

            var sku = await _repository.First<Product>(p => p.Sku == request.Sku && p.IsActive);
            if (sku != null) throw new DuplicatedEntityException($"Un producto con Sku {request.Sku} ya existe");

            exist.Sku = request.Sku;
            exist.InternalCode = request.InternalCode;
            exist.Name = request.Name;
            exist.Description = request.Description;
            exist.CurrentUnitPrice = request.CurrentUnitPrice;
            exist.StockQuantity = request.StockQuantity;

            await _repository.Update(exist);

            return new ProductModel.ResponseProductModel
           (
                exist.Id,
                exist.Sku,
                exist.InternalCode,
                exist.Name,
                exist.Description,
                exist.CurrentUnitPrice,
                exist.StockQuantity,
                exist.IsActive
            );
        }

        public async Task PatchProduct(Guid id)
        {
            var exist = await _repository.GetById<Product>(id);
            if (exist == null)
                throw new EntityNotFoundException("Product not found");
            if (exist.IsActive == false)
                throw new ApplicationException("Producto ya deshabilitado");
            exist.IsActive = false;
            await _repository.Update(exist);
        }
    }
}

