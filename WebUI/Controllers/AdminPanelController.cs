using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AdminPanelController : Controller
    {
        private IMenuService _menuService;
        private IAuthService _authService;

        public AdminPanelController(IMenuService menuService, IAuthService authService)
        {
            _menuService = menuService;
            _authService = authService;
        }

        [Authorize(Roles = Roles.Admin)]
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
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Add(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _menuService.Add(menuItem);
                    TempData.Add("message", "Item successfully updated!");

                }
                catch (Exception ex)
                {
                    string error = ex.InnerException.Message;
                    if (error.Contains("Cannot insert duplicate key row in object"))
                        TempData.Add("message", "Insertion failed! " + menuItem.Name + " is aready exist!");
                    else
                        TempData.Add("message", "Insertion failed! " + error);
                }
            }
            else
            {
                TempData.Add("message", "Insertion failed, invalid inputs!");
            }
            return RedirectToAction("Menu");
        }

        [Route("/adminpanel/menu/update")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int itemId)
        {
            var model = new AdminPanelMenuUpdateModel
            {
                MenuItem = _menuService.GetById(itemId)
            };
            return View(model);
        }

        [Route("/adminpanel/menu/update")]
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Update(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*var oldItem = _menuService.GetById(menuItem.ItemId);
                    oldItem.Name = menuItem.Name;
                    oldItem.Category = menuItem.Category;
                    oldItem.IsAvailable = menuItem.IsAvailable;
                    oldItem.Price = menuItem.Price;
                    oldItem.Explanation = menuItem.Explanation;*/
                    _menuService.Update(menuItem);
                    TempData.Add("message", "Item successfully updated!");

                } catch (Exception ex)
                {
                    string error = ex.InnerException.Message;
                    string oldName = _menuService.GetById(menuItem.ItemId).Name;
                    if (error.Contains("Cannot insert duplicate key row in object"))
                        TempData.Add("message", "Update failed! Can not rename " + oldName + " to " + menuItem.Name + ". Because " + oldName + " already exist in menu");
                    else
                        TempData.Add("message", "Update failed! " + error);
                }
            }
            else
            {
                TempData.Add("message", "Update failed, invalid inputs!");
            }
            return RedirectToAction("Menu");
        }

        [Route("/adminpanel/menu/delete")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int itemId)
        {
            try
            {
                _menuService.Delete(itemId);
                TempData.Add("message", "Item successfully deleted!");
            } catch(Exception ex)
            {
                string error = ex.InnerException.Message;
                TempData.Add("message", "Delete failed! " + error);
            }
            return RedirectToAction("Menu");
        }
    }
}