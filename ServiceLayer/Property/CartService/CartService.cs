using DomainLayer;
using RepositoryLayer.Infrascructure.Company;
using RepositoryLayer.Infrascructure.Products;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Infrascructure.Cart;
using ServiceLayer.Property.UserProfileService;
using RepositoryLayer.Infrascructure.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Property.CartService
{
    public class CartService : ICartService
    {
        private ICart<Cart> _cart;
        private IProducts<Products> _products;
        private IUserLogic<User> _userLogic;
        //private IUserLogic<User> _userprofile;
        private readonly ApplicationContext _context;
        public CartService(IProducts<Products> products, ICart<Cart> cart, IUserLogic<User> user)
        {
            this._cart = cart;
            this._products = products;
            this._userLogic = user;
        }


        public IEnumerable<Cart> GetAll()
        {
            return _cart.GetAllCart();
        }
        public IEnumerable<Cart> GetProductInCart(/*long Id*/string name)
        {
            //string? id = User(ClaimTypes.NameIdentifier);
            //int id2 = Convert.ToInt32(id);
            var user = _userLogic.GetAll().Where(x => x.UserName == name).FirstOrDefault();
            List<Cart> cart = (List<Cart>)_cart.GetAllCart().Where(x => x.UserProfileId == user.Id).ToList();
            return cart;
        }

        public void AddProductToCart(Cart cart)
        {
            _cart.AddToCart(cart);
        }
        public void EditCount(Cart cart)
        {
            _cart.EditCount(cart);
        }
        public Cart Get(long id)
        {
            return _cart.Get(id);
        }

        public void DeleteFromCart(long id)
        {
            //List<Products> product = (List<Products>)_products.GetAll().Where(x => x.CompanyId == id).ToList();
            //if (product != null)
            //{
            //    _products.RemoveAll(product);
            //}
            Cart cart = _cart.Get(id);
            _cart.DeleteFromCart(cart);
            _cart.SaveChanges();
        }
    }
}
