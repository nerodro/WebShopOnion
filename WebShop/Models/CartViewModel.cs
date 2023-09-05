using Microsoft.AspNetCore.Mvc;

namespace WebShop.Models
{
    public class CartViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
    }
}
