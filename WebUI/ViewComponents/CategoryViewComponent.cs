using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        IMenuService _menuService;
        public CategoryViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ViewViewComponentResult Invoke()
        {
            var model = new CategoryViewModel
            {
                Categories = _menuService.GetCategories(),
                CurrentCategory = HttpContext.Request.Query["category"]
            };
            return View(model);
        }
    }
}
