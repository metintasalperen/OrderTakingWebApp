using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AdminPanelController : Controller
    {
        private IMenuService _menuService;

        public AdminPanelController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public IActionResult Index(string category = "none")
        {
            var menu = _menuService.GetAll();
            AdminPanelMenuModel model = new AdminPanelMenuModel
            {
                Menu = menu,
                Categories = _menuService.GetCategories(),
                CurrentCategory = category
            };
            return View(model);
        }
    }
}