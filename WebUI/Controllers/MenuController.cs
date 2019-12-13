using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using Entities.Dtos;
using WebUI.ExtensionMethods;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public IActionResult Index(int table, string category = "none")
        {
            //int pageSize = 10;
            var menu = _menuService.GetByCategory(category);
            List<MenuItemBasketDto> basket = SessionExtensionMethods.GetObject<List<MenuItemBasketDto>>(HttpContext.Session,"basket");
            if (basket == null)
                basket = new List<MenuItemBasketDto>();

            MenuViewModel model = new MenuViewModel
            {
                //Menu = menu.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Menu = menu,
                Categories = _menuService.GetCategories(),
                CurrentCategory = category,
                Basket = basket
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(MenuItemBasketDto item, int table, string category = "none")
        {
            List<MenuItemBasketDto> basket =
                SessionExtensionMethods.GetObject<List<MenuItemBasketDto>>(HttpContext.Session, "basket");

            if(basket == null)
                basket = new List<MenuItemBasketDto>();

            bool exists = false;
            foreach (var i in basket)
            {
                if (i.itemId == item.itemId)
                {
                    if (item.isUpdate == 0)
                    {
                        exists = true;
                        i.quantity += item.quantity;
                    }
                    else
                    {
                        i.quantity = item.quantity;
                        if (i.quantity > 0)
                            exists = true;
                    }
                }
            }

            if (!exists)
            {
                basket.Add(item);
            }
            SessionExtensionMethods.SetObject(HttpContext.Session,"basket", basket);

            var menu = _menuService.GetByCategory(category);
            MenuViewModel model = new MenuViewModel
            {
                Menu = menu,
                Categories = _menuService.GetCategories(),
                CurrentCategory = category,
                Basket = basket
            };
            return View(model);
        }
    }
}
