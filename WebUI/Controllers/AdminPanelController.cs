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
        public IActionResult Menu(string category = "none")
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

        [Route("/adminpanel/menu/add")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("/adminpanel/menu/add")]
        [HttpPost]
        public IActionResult Add(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _menuService.Add(menuItem);
                TempData.Add("message", "Item successfully added!");
            }
            else
            {
                TempData.Add("message", "Insertion failed!");
            }
            return RedirectToAction("Menu");
        }

        [Route("/adminpanel/menu/update")]
        public IActionResult Update(int itemId)
        {
            var model = new AdminPanelMenuUpdateModel
            {
                MenuItem = _menuService.GetById(itemId)
            };
            return View(model);
        }

        [Route("/adminpanel/menu/update")]
        [HttpPost]
        public IActionResult Update(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _menuService.Update(menuItem);
                TempData.Add("message", "Item successfully updated!");
            }
            else
            {
                TempData.Add("message", "Update failed!");
            }
            return RedirectToAction("Menu");
        }
    }
}