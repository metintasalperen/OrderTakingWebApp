using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
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
        private ITableService _tableService;
        private IUserService _userService;
        public MenuController(IMenuService menuService, ITableService tableService, IUserService userService)
        {
            _menuService = menuService;
            _tableService = tableService;
            _userService = userService;
        }
        public IActionResult Index(int table, string category = "none")
        {
            //assign waiter
            Table current_table = _tableService.GetByTableId(table);
            if (current_table == null)
                return BadRequest();

            //TODO get token and compare if not emtpty
            if (current_table.IsEmpty)
            {
                //current_table.IsEmpty = false;
                //_tableService.Update(current_table);
            }
            
            var menu = _menuService.GetByCategory(category);
            List<MenuItemBasketDto> basket = SessionExtensionMethods.GetObject<List<MenuItemBasketDto>>(HttpContext.Session,"basket");
            if (basket == null)
                basket = new List<MenuItemBasketDto>();

            MenuViewModel model = new MenuViewModel
            {
                TableNumber = table,
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
                TableNumber = table,
                Menu = menu,
                Categories = _menuService.GetCategories(),
                CurrentCategory = category,
                Basket = basket
            };
            return View(model);
        }
    }
}
