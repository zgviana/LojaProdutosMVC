using Loja.Entidades;

namespace Loja.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}
