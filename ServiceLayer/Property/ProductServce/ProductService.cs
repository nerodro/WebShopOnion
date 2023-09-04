using DomainLayer;
using RepositoryLayer.Infrascructure.Company;
using RepositoryLayer.Infrascructure.Products;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Property.ProductServce
{
    public class ProductService : IProductService
    {
        private IProducts<Products> _products;
        private ICompany<Company> _company;
        private readonly ApplicationContext _context;
        public ProductService(IProducts<Products> products, ICompany<Company> company)
        {
            this._company = company;
            this._products = products;
        }

        public IEnumerable<Products> GetAll()
        {
            return _products.GetAll();
        }

        public Company GetCompany(long id)
        {
            return _context.Company.FirstOrDefault(x => x.Id == id);
        }

        public Products Get(long id)
        {
            return _products.Get(id);
        }

        public void Create(Products products)
        {
            _products.Create(products);
        }
        public void Update(Products products)
        {
            _products.Update(products);
        }

        public void Delete(long id)
        {
            Products product = _products.Get(id);
            _products.Remove(product);
        }
        public void DeleteAll(long id)
        {
            List<Products> products = _context.Products.Where(x => x.CompanyId == id).ToList();
            _products.RemoveAll(products);
        }
    }
}
