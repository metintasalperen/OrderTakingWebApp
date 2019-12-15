using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _menuService.Add(menuItem);
                TempData.Add("message", "Item successfully added!");
            }
            return View();
        }
    }
}