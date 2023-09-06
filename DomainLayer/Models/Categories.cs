using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Categories : BaseEntity
    {
        public string CategoryName { get; set; }
        public string categoryico { get; set; }
        public  List<Products> Products { get; set; }
        public Categories() 
        { 
            Products = new List<Products>();
        }
    }
}
