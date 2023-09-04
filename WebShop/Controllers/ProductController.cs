using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;
using ServiceLayer.Property.CompanyService;
using ServiceLayer.Property.ProductServce;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICompanyService _company;
        private readonly IProductService _product;
        private ApplicationContext context;

        public ProductController(ICompanyService company, IProductService product)
        {
            this._company = company;
            this._product = product;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            if (_product != null)
            {
                _product.GetAll().ToList().ForEach(u =>
                {
                    Company company = _company.Get(u.CompanyId);
                    ProductViewModel product = new ProductViewModel()
                    {
                        Id = u.Id,
                        ProductName = u.ProductName,
                        ProductNumber = u.ProductNumber,
                        Price = u.Price,
                        ProductDescription = u.ProductDescription,
                        CompanyName = company.CompanyNamme
                    };
                    model.Add(product);
                });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            ProductViewModel model = new ProductViewModel();

            return View("AddProduct", model);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model)
        {
            Products products = new Products
            {
                ProductName = model.ProductName,
                CompanyId = model.CompanyId,
                Price = model.Price,
                ProductDescription = model.ProductDescription,
                ProductNumber = model.ProductNumber,
            };
            _product.Create(products);
            if (products.Id > 0)
            {
                return RedirectToAction("index");
            }
            return View(products);
        }

        public ActionResult EditProduct(int? id)
        {
            ProductViewModel model = new ProductViewModel();
            if (id.HasValue && id != 0)
            {
                Products products = _product.Get(id.Value);
                model.ProductNumber = products.ProductNumber;
                model.ProductName = products.ProductName;
                model.ProductDescription = products.ProductDescription;
                model.CompanyId = products.CompanyId; 
                model.Price = products.Price;
            }
            return View("EditProduct", model);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model)
        {
            Products products = _product.Get(model.Id);
            products.CompanyId = model.CompanyId;
            products.ProductNumber = model.ProductNumber;
            products.ProductDescription = model.ProductDescription;
            products.ProductName = model.ProductName;
            products.Price = model.Price;
            products.ModifiedDate = DateTime.UtcNow;
            _product.Update(products);
            if (products.Id > 0)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            Products products = _product.Get(id);
            string name = $"{products.ProductName} {products.ProductNumber}";
            return View("DeleteProduct", name);
        }

        [HttpPost]
        public ActionResult DeleteProduct(long id)
        {
            _product.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ProductProfile(int? id)
        {
            
            ProductViewModel model = new ProductViewModel();

            if (id.HasValue && id != 0)
            {
                Products products = _product.Get(id.Value);
                Company company = _company.Get(products.CompanyId);
                model.ProductNumber = products.ProductNumber;
                model.ProductDescription = products.ProductDescription;
                model.ProductName = products.ProductName;
                model.Price = products.Price;
                model.CompanyName = company.CompanyNamme;
            }
            return View("ProductProfile", model);
        }
        [HttpPost]
        public ActionResult ProductProfile(ProductViewModel model)
        {
            Products products = _product.Get(model.Id);
            Company company = _company.Get(products.CompanyId);
            products.ProductName = model.ProductName;
            products.Price = model.Price;
            products.ProductDescription = model.ProductDescription;
            company.CompanyNamme = model.CompanyName;
            return View(model);
        }
    }
}
