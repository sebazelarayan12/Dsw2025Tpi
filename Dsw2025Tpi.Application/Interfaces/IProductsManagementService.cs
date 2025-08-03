using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IProductsManagementService
    {
        Task<ProductModel.ResponseProductModel?> GetProductById(Guid id);
        Task<IEnumerable<ProductModel.ResponseProductModel>?> GetAllProducts();
        Task<ProductModel.ResponseProductModel> AddProduct(ProductModel.RequestProductModel request);
        Task<ProductModel.ResponseProductModel> UpdateProduct(Guid id, ProductModel.RequestProductModel request);
        Task PatchProduct(Guid id);
    }
}
