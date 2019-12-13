using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public IActionResult Index(int page = 1, string category = "none")
        {
            int pageSize = 10;
            var menu = _menuService.GetByCategory(category);
            MenuViewModel model = new MenuViewModel
            {
                Menu = menu.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Categories = _menuService.GetCategories(),
                CurrentCategory = category
            };
            return View(model);
        }
    }
}
