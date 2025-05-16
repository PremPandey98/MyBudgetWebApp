using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                
               //var existingProduct = context.Products.Find(product.Id);
               var existingProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                product.CreatedAt = existingProduct.CreatedAt; // preserve original CreatedAt
                existingProduct.UpdatedAt = DateTime.Now;
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Stock = product.Stock;
                context.Products.Update(existingProduct);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
