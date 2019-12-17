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
using System.Security.Claims;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService _menuService;
        private ITableService _tableService;
        private IUserService _userService;
        private IAuthService _authService;
        private IOrderService _orderService;
        public MenuController(IMenuService menuService, ITableService tableService, IUserService userService, IAuthService authService, IOrderService orderService)
        {
            _menuService = menuService;
            _tableService = tableService;
            _userService = userService;
            _authService = authService;
            _orderService = orderService;
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

            List<User> waiters = _userService.GetByRole("Waiter");
            waiters = waiters.OrderBy(o => o.CountOfTable).ToList();
            int waiter_id = waiters[0].UserId;
            var waiter = _userService.GetByUserId(waiter_id);
            waiter.CountOfTable++;
            _userService.Update(waiter);
            if (!current_table.IsApproved)
            {
                var tableOrders = _orderService.GetByTableId(current_table.TableId);
                bool dummy = false;
                foreach(var item in tableOrders)
                {
                    if (item.IsDummy)
                    {
                        dummy = true;
                        break;
                    }
                }
                if (!dummy)
                {
                    Order order = new Order
                    {
                        ItemId = _menuService.GetAll().ElementAt(0).ItemId,
                        TableId = current_table.TableId,
                        WaiterId = waiter_id,
                        Quantity = 1,
                        IsDelivered = false,
                        IsDummy = true,
                    };
                    _orderService.Add(order);
                }
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
                Basket = basket,
                IsApproved = _tableService.GetByTableId(table).IsApproved
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
                Basket = basket,
                IsApproved = _tableService.GetByTableId(table).IsApproved
            };
            return View(model);
        }
    }
}
