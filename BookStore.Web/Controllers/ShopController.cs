using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ShopController(ApplicationDbContext context)
        {
            applicationDbContext = context;
        }
        public IActionResult Index()
        {
            var authors = applicationDbContext.Authors.OrderBy(a => a.AuthorName).ToList();
            return View(authors);
        }
        public IActionResult Browse(int author)
        {
            ViewBag.author = applicationDbContext.Authors.Where(a => a.AuthorID == author).FirstOrDefault();
            var books = applicationDbContext.Books.Where(a => a.AuthorId == author).OrderBy(b => b.BookName).ToList();
            return View(books);
        }

        public IActionResult BookDetails(int book)
        {
            var books = applicationDbContext.Books.SingleOrDefault(a => a.BookId == book);
            ViewBag.author = applicationDbContext.Authors.Where(a => a.AuthorID == books.AuthorId).FirstOrDefault();
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int Quantity, int book)
        {
            var books = applicationDbContext.Books.SingleOrDefault(a => a.BookId == book);
            var price = books.Price;
            var cartusername = GetCartUserName();
            var cart = new Cart
            {
                BookId = book,
                Quantity = Quantity,
                Price = price,
                username = cartusername
            };
            applicationDbContext.carts.Add(cart);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Cart");
        }

        private string GetCartUserName()
        {
            if(HttpContext.Session.GetString("CartUserName")  == null)
            {
                var cartusername = "";
                if(User.Identity.IsAuthenticated)
                {
                    cartusername = User.Identity.Name;

                }
                else {
                    cartusername = Guid.NewGuid().ToString();
                }
                HttpContext.Session.SetString("CartUserName", cartusername);
            }
            return HttpContext.Session.GetString("CartUserName");
        }
        public IActionResult Cart()
        {
            var cartusername = GetCartUserName();
            var cart = applicationDbContext.carts.Where(a => a.username == cartusername).ToList();
            return View(cart);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cartitem = applicationDbContext.carts.SingleOrDefault(c => c.cartId == id);
            applicationDbContext.carts.Remove(cartitem);
            applicationDbContext.SaveChanges();

            return RedirectToAction("Cart");

        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
