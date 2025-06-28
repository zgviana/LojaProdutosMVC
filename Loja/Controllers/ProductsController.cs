using Loja.Entidades;
using Loja.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Loja.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly string cookie = "ProdutosFavoritos";

        //Injeção de dependencia no IProductService
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()        
        {
            HttpRequest req = Request;

            try
            {
                //busca todos os produtos
                var products = await _productService.GetAllProductsAsync();

                //Verificando se os produtos buscados estão gravados no cookie como favoritos
                foreach (var product in products)
                {
                    if (FindProductFavoritosCoockie(product.Id, req))
                        product.Favorito = true;
                    else
                        product.Favorito = false;
                }

                return View(products);
            }
            catch (Exception ex) { 
                ViewBag.MensagemErro = "Ocorreu um erro! Tente novamente mais tarde.";
                return View();
            }           
        }       

        //Verifica se o produto é favoritado no cookies
        public bool FindProductFavoritosCoockie(int id, HttpRequest req)
        {
            List<int> favoritos;
            bool existFavorito = false;

            if (Request.Cookies.ContainsKey(cookie))
            {
                var cookieValue = Request.Cookies[cookie];
                favoritos = cookieValue.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                favoritos = new List<int>();
            }

            if (favoritos.Contains(id))
                existFavorito = true;
            else
                existFavorito = false;

            return existFavorito;
        }

        public async Task<IActionResult> Details(int id, string paginaAtual)
        {
            ViewBag.paginaAtual = paginaAtual;
            HttpRequest req = Request;
            var products = await _productService.GetAllProductsAsync();
            //buscando na lista o produto por ID
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                // return NotFound();
                ViewBag.MensagemErro = "Produto não encontrado.";
                return View();
            }

            if (FindProductFavoritosCoockie(product.Id, req))
                product.Favorito = true;
            else
                product.Favorito = false;


            return View(product);
        }

        public async Task<IActionResult> Favoritos()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                List<Product> productsFavoritos = new List<Product>();

                HttpRequest req = Request;

                foreach (var product in products)
                {
                    if (FindProductFavoritosCoockie(product.Id, req))
                    {
                        product.Favorito = true;
                        productsFavoritos.Add(product);
                    }
                }

                return View(productsFavoritos);
            }
            catch (Exception ex) {
                ViewBag.MensagemErro = "Ocorreu um erro! Tente novamente mais tarde.";
                return View();
            }
        }

        public IActionResult AddFavoritos(int id, bool indFavoritos, string paginaRedirect)
        {            
            List<int> favoritos;           

            if (Request.Cookies.ContainsKey(cookie))
            {
                var cookieValue = Request.Cookies[cookie];
                favoritos = cookieValue.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                favoritos = new List<int>();
            }

            //Se ele já é favoritado e ta clicando denovo em favoritos, vamos retirar ele do favoritos
            if (indFavoritos)
            {
                if (favoritos.Contains(id))
                    favoritos.Remove(id);
            }
            else
            {
                if (!favoritos.Contains(id))
                    favoritos.Add(id);
            }

            // Salvar o cookie novamente
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30) // Duração de 30 dias
            };
            //Gravando favoritos
            Response.Cookies.Append(cookie, string.Join(",", favoritos), cookieOptions);

            return RedirectToAction(paginaRedirect);
        }
    }
}
