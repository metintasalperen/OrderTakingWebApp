using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using Entities.Dtos;
using WebUI.ExtensionMethods;
using RestSharp;
using System.Security.Claims;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService _menuService;
        private ITableService _tableService;
        private IUserService _userService;
        private IAuthService _authService;
        private IHttpContextAccessor _httpContextAccessor;
        public MenuController(IMenuService menuService, ITableService tableService, IUserService userService, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _menuService = menuService;
            _tableService = tableService;
            _userService = userService;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index(int table, string category = "none")
        {
            //assign waiter
            Table current_table = _tableService.GetByTableId(table);
            if (current_table == null)
                return BadRequest();
            if (current_table.IsEmpty)
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                string new_token = _authService.CreateAccessTokenForCustomer(table, "Customer").Data.Token;
                current_table.Token = new_token;
                Response.Cookies.Append("token", new_token, cookie);
                current_table.IsEmpty = false;
                _tableService.Update(current_table);
            }
            else if (HttpContext.Request.Cookies.ContainsKey("token"))
            {
                string request_token = HttpContext.Request.Cookies["token"];
                if (!current_table.Token.Equals(request_token))
                    return Unauthorized();
            }
            else
            {
                return Unauthorized();
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
            Table current_table = _tableService.GetByTableId(table);
            if (current_table == null || current_table.IsEmpty)
                return BadRequest();
            else if (HttpContext.Request.Cookies.ContainsKey("token"))
            {
                string request_token = HttpContext.Request.Cookies["token"];
                if (!current_table.Token.Equals(request_token))
                    return Unauthorized();
            }
            else
            {
                return Unauthorized();
            }

            List<MenuItemBasketDto> basket =
                SessionExtensionMethods.GetObject<List<MenuItemBasketDto>>(HttpContext.Session, "basket");

            if(basket == null)
                basket = new List<MenuItemBasketDto>();

            bool exists = false;
            MenuItemBasketDto zero = null;
            foreach (var i in basket)
            {
                if (i.itemId == item.itemId)
                {
                    exists = true;
                    if (item.isUpdate == 0)
                    {
                        i.quantity += item.quantity;
                    }
                    else
                    {
                        i.quantity = item.quantity;
                        if (i.quantity == 0)
                            zero = i;
                    }
                }
            }

            if (zero != null)
            {
                basket.Remove(zero);
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
