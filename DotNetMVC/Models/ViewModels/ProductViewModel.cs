using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace DotNetMVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
