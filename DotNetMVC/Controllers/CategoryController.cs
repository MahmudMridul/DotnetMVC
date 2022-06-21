using DotNetMVC.Data;
using DotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        //GET - EDIT
        public IActionResult Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var obj = _db.Categories.Find(id);

            if(obj == null)
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
