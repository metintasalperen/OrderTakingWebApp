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
        public IActionResult Index()
        {
            var menu = _menuService.GetAll();
            MenuViewModel model = new MenuViewModel
            {
                Menu = menu
            };
            return View(model);
        }
    }
}
