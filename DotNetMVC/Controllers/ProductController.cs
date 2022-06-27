using DotNetMVC.Data;
using DotNetMVC.Models;
using DotNetMVC.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            IEnumerable<Product> Products = _db.Products;

            foreach(var product in Products)
            {
                product.Category = _db.Categories.SingleOrDefault
                (
                    category => category.Id == product.CategoryId
                );
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
        public IActionResult Upsert(ProductViewModel ProductVMObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(ProductVMObj.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WebConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    ProductVMObj.Product.Image = fileName + extension;

                    _db.Products.Add(ProductVMObj.Product);

                }
                else
                {
                    //update
                    var ProductFromDb = _db.Products.AsNoTracking().FirstOrDefault
                        (
                            product => product.Id == ProductVMObj.Product.Id
                        );

                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, ProductFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        ProductVMObj.Product.Image = fileName + extension;
                    }
                    else
                    {
                        ProductVMObj.Product.Image = ProductFromDb.Image;
                    }

                    _db.Products.Update(ProductVMObj.Product);
                }

                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
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
