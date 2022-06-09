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
    }
}
