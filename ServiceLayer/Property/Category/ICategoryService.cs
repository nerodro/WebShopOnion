using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Property.Category
{
    public interface ICategoryService
    {
        IEnumerable<Categories> GetAll();
        IEnumerable<Products> GetAllProduct(long id);
        Categories Get(long id);
        void Create(Categories categories);
        void Update(Categories categories);
        void Delete(long id);
    }
}
