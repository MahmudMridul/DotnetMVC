using DotNetMVC.Data;
using DotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationContext _db;

        public CategoryController(ApplicationContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            IEnumerable<Category> Categories = _db.Categories;
            return View(Categories);
        }

        //GET -CREATE
        public IActionResult Create()
        {

            return View();
        }

        //POST -CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category CategoryObj)
        {
            if(ModelState.IsValid)
            {
                _db.Categories.Add(CategoryObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CategoryObj);
        }
    }
}
