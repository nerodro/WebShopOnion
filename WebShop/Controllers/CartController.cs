using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;
using RepositoryLayer.Infrascructure.Company;
using ServiceLayer.Property.CartService;
using ServiceLayer.Property.CompanyService;
using ServiceLayer.Property.ProductServce;
using ServiceLayer.Property.UserService;
using System.Security.Claims;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _product;
        private readonly ICartService _cart;
        private readonly IUserService _user;
        private ApplicationContext context;

        public CartController(IProductService product, ICartService cart, IUserService user)
        {
            this._cart= cart;
            this._product = product;
            this._user = user;
        }

        [HttpGet]
        //[Authorize(Roles = "admin, moder,user")]
        public IActionResult Index()
        {
            string Id = User.FindFirst(ClaimTypes.Name).Value;
            List<CartViewModel> model = new List<CartViewModel>();
            if (_cart != null)
            {
                _cart.GetProductInCart(Id).ToList().ForEach(u =>
                {
                    CartViewModel cart = new CartViewModel
                    {
                        ProductId = (int)u.ProductsId
                    };
                    model.Add(cart);
                });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddToCart(int? Id)
        {
            CartViewModel cart = new CartViewModel();

            //return RedirectToAction("Index", "Cart");
            return View("AddToCart", cart);
        }

       
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartViewModel model, int? Id)
        {
            //string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //int id2 = Convert.ToInt32(id);
            //Products product = context.Products.Find(Id);
            Cart cart = new Cart
            {
                ProductsId = model.ProductId,
                UserProfileId = model.UserId,
                //ProductId = model.ProductId,
            };
            _cart.AddProductToCart(cart);
            if (cart.Id > 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            return View(model);
            //return RedirectToAction("Index", "Cart");
        }

        //public ActionResult EditCompany(int? id)
        //{
        //    CompanyViewModel model = new CompanyViewModel();
        //    if (id.HasValue && id != 0)
        //    {
        //        Company company = _company.Get(id.Value);
        //        model.CompanyIdentity = company.CompanyIdentity;
        //        model.CompanyNamme = company.CompanyNamme;
        //    }
        //    return View("EditCompany", model);
        //}

        //[HttpPost]
        //public ActionResult EditCompany(CompanyViewModel model)
        //{
        //    Company company = _company.Get(model.Id);
        //    company.CompanyNamme = model.CompanyNamme;
        //    company.ModifiedDate = DateTime.UtcNow;
        //    company.CompanyIdentity = model.CompanyIdentity;
        //    _company.Update(company);
        //    if (company.Id > 0)
        //    {
        //        return RedirectToAction("index");
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult DeleteCompany(int id)
        //{
        //    Company company = _company.Get(id);
        //    string name = $"{company.CompanyNamme} {company.CompanyIdentity}";
        //    return View("DeleteCompany", name);
        //}

        //[HttpPost]
        //public ActionResult DeleteCompany(long id)
        //{
        //    _company.Delete(id);
        //    return RedirectToAction("Index");
        //}
        //[HttpGet]
        //public ActionResult CompanyProfile(int? id)
        //{
        //    CompanyViewModel model = new CompanyViewModel();
        //    if (id.HasValue && id != 0)
        //    {
        //        Company company = _company.Get(id.Value);
        //        model.CompanyIdentity = company.CompanyIdentity;
        //        model.CompanyNamme = company.CompanyNamme;
        //    }
        //    return View("CompanyProfile", model);
        //}
        //[HttpPost]
        //public ActionResult CompanyProfile(CompanyViewModel model)
        //{
        //    Company company = _company.Get(model.Id);
        //    company.CompanyIdentity = model.CompanyIdentity;
        //    company.CompanyNamme = model.CompanyNamme;
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult AddToCart(CartViewModel model)
        //{
        //    string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    int id2 = Convert.ToInt32(id);
        //    Cart cart = new Cart
        //    {
        //        UserProfileId = id2,
        //        ProductId = model.ProductId,
        //    };
        //    //Company company1 = new Company
        //    //{
        //    //    CompanyNamme = company.CompanyNamme,
        //    //    CompanyIdentity = company.CompanyIdentity,
        //    //};
        //    //_company.Create(company1);
        //    _cart.AddProductToCart(cart);
        //    if (cart.Id > 0)
        //    {
        //        return RedirectToAction("index");
        //    }
        //    return View(model);
        //}
    }
}
