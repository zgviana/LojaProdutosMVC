using Loja.Entidades;
using Loja.Repositories.Interface;
using Loja.Services.Interfaces;

namespace Loja.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}
