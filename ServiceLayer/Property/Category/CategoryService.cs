using DomainLayer.Models;
using RepositoryLayer.Infrascructure.Company;
using RepositoryLayer.Infrascructure.Products;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Infrascructure.Categories;

namespace ServiceLayer.Property.Category
{
    public class CategoryService : ICategoryService
    {
        private IProducts<Products> _products;
        private ICategories<Categories> _categoryService;
        private readonly ApplicationContext _context;
        public CategoryService(IProducts<Products> products, ICategories<Categories> categories)
        {
            this._products = products;
            this._categoryService = categories;
        }

        public IEnumerable<Products> GetAllProduct(long Id)
        {
            return _context.Products.Where(x => x.CategorysId == Id);
        }

        public IEnumerable<Categories> GetAll()
        {
            return _categoryService.GetAllCategories();
        }

        public Categories Get(long id)
        {
            return _categoryService.Get(id);
        }

        public void Create(Categories categories)
        {
            _categoryService.AddCategory(categories);
        }
        public void Update(Categories categories)
        {
            _categoryService.EditCategory(categories);
        }

        public void Delete(long id)
        {
            Categories categories = _categoryService.Get(id);
            _categoryService.DeleteCategory(categories);
            _categoryService.SaveChanges();
        }
    }
}
