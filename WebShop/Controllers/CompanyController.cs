using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;
using RepositoryLayer.Infrascructure.Company;
using ServiceLayer.Property.CompanyService;
using ServiceLayer.Property.ProductServce;
using System.Data;
using TestOnion.Models;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _company;
        private readonly IProductService _product;
        private ApplicationContext context;

        public CompanyController(ICompanyService company, IProductService product)
        {
            this._company = company;
            this._product = product;
        }

        [HttpGet]
        //[Authorize(Roles = "admin, moder,user")]
        public IActionResult Index()
        {
            List<CompanyViewModel> model = new List<CompanyViewModel>();
            if (_company != null)
            {
                _company.GetAll().ToList().ForEach(u =>
                {
                    CompanyViewModel company = new CompanyViewModel
                    {
                        Id = u.Id,
                        CompanyIdentity = u.CompanyIdentity,
                        CompanyNamme = u.CompanyNamme,
                    };
                    model.Add(company);
                });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddCompany()
        {
            CompanyViewModel company = new CompanyViewModel();

            return View("AddCompany", company);
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyViewModel company)
        {
            Company company1 = new Company
            {
                CompanyNamme = company.CompanyNamme,
                CompanyIdentity = company.CompanyIdentity,
            };
            _company.Create(company1);
            if (company1.Id > 0)
            {
                return RedirectToAction("index");
            }
            return View(company);
        }

        public ActionResult EditCompany(int? id)
        {
            CompanyViewModel model = new CompanyViewModel();
            if (id.HasValue && id != 0)
            {
                Company company = _company.Get(id.Value);
                model.CompanyIdentity = company.CompanyIdentity;
                model.CompanyNamme = company.CompanyNamme;
            }
            return View("EditCompany", model);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyViewModel model)
        {
            Company company = _company.Get(model.Id);
            company.CompanyNamme = model.CompanyNamme;
            company.ModifiedDate = DateTime.UtcNow;
            company.CompanyIdentity = model.CompanyIdentity;
            _company.Update(company);
            if (company.Id > 0)
            {
                return RedirectToAction("index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteCompany(int id)
        {
            Company company = _company.Get(id);
            string name = $"{company.CompanyNamme} {company.CompanyIdentity}";
            return View("DeleteCompany", name);
        }

        [HttpPost]
        public ActionResult DeleteCompany(long id)
        {
            _company.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CompanyProfile(int? id)
        {
            CompanyViewModel model = new CompanyViewModel();
            if (id.HasValue && id != 0)
            {
                Company company = _company.Get(id.Value);
                model.CompanyIdentity = company.CompanyIdentity;
                model.CompanyNamme = company.CompanyNamme;
            }
            return View("CompanyProfile", model);
        }
        [HttpPost]
        public ActionResult CompanyProfile(CompanyViewModel model)
        {
            Company company = _company.Get(model.Id);
            company.CompanyIdentity = model.CompanyIdentity;
            company.CompanyNamme = model.CompanyNamme;
            return View(model);
        }
    }
}
