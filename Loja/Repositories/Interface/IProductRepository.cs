using Loja.Entidades;

namespace Loja.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
    }
}
