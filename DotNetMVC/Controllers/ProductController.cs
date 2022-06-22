using DotNetMVC.Data;
using DotNetMVC.Models;
using DotNetMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _db;

        public ProductController(ApplicationContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            IEnumerable<Product> Products = _db.Products;

            foreach(var product in Products)
            {
                product.Category = _db.Categories.Find(product.CategoryId);
            }

            return View(Products);
        }

        //GET -CREATE
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropdown = _db.Categories.Select
            //(
            //    item => new SelectListItem
            //    {
            //        Text = item.Name,
            //        Value = item.Id.ToString()
            //    }
            //);

            //ViewBag.CategoryDropdown = CategoryDropdown;



            //Product product = new Product();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _db.Categories.Select
                (
                    item => new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                )
            };

            if(id == null)
            {
                //this is for create
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _db.Products.Find(id);

                if(productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }

        //POST -CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product ProductObj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(ProductObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ProductObj);
        }

        //GET - EDIT
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category CategoryObj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(CategoryObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CategoryObj);
        }

        //GET - DELETE
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
