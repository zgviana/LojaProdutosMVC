using Loja.Entidades;
using Loja.Repositories.Interface;
using System.Text.Json;


namespace Loja.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetAllAsync()
        {
            using (var httpClient = new HttpClient())
            {
                List<Product>? listProduct = new List<Product>();

                var response = await httpClient.GetStringAsync("https://fakestoreapi.com/products");
                listProduct = JsonSerializer.Deserialize<List<Product>>(response); //JsonConvert.DeserializeObject<List<Product>>(response);
                return listProduct;
            }
        }
    }
}
