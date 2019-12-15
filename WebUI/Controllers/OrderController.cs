using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebUI.ExtensionMethods;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        private IMenuService _menuService;
        private IUserService _userService;
        public OrderController(IOrderService orderService, IMenuService menuService, IUserService userService)
        {
            _orderService = orderService;
            _menuService = menuService;
            _userService = userService;
        }

        public IActionResult Index(int table)
        {
            List<Order> orders = _orderService.GetByTableId(table);

            if (orders == null)
            {
                orders = new List<Order>();
            }

            List<Order> deliveredOrders = new List<Order>();
            List<Order> notDeliveredOrders = new List<Order>();

            foreach (var order in orders)
            {
                order.MenuItem = _menuService.GetById(order.ItemId);
                if (order.IsDelivered)
                    deliveredOrders.Add(order);
                else
                    notDeliveredOrders.Add(order);
            }

            OrderViewModel model = new OrderViewModel()
            {
                TableNumber = table,
                DeliveredOrders = deliveredOrders,
                NotDeliveredOrders = notDeliveredOrders
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int table, int dummy)
        {
            List<MenuItemBasketDto> basket = SessionExtensionMethods.GetObject<List<MenuItemBasketDto>>(HttpContext.Session, "basket");
            List<Order> current_orders = _orderService.GetByTableId(table);
            int waiter_id;

            if (current_orders.Count == 0)
            {
                List<User> waiters = _userService.GetByRole("Waiter");
                waiters = waiters.OrderBy(o => o.CountOfTable).ToList();
                waiter_id = waiters[0].UserId;
            }
            else
            {
                waiter_id = current_orders[0].WaiterId;
            }

            foreach (var item in basket)
            {
               Order order = _orderService.GetByTableIdAndItemId(table, item.itemId);
               if (order == null || order.IsDelivered)
               {
                   if (order == null)
                   {
                       User waiter = _userService.GetByUserId(waiter_id);
                       waiter.CountOfTable++;
                       _userService.Update(waiter);
                   }

                   MenuItem ordered_item = _menuService.GetById(item.itemId);
                   ordered_item.Sold += item.quantity;
                   _menuService.Update(ordered_item);
                   _orderService.Add(new Order()
                   {
                       IsDelivered = false,
                       ItemId = item.itemId,
                       Quantity = item.quantity,
                       TableId = table,
                       WaiterId = waiter_id
                   });
               }
               else
               {
                   MenuItem ordered_item = _menuService.GetById(item.itemId);
                   ordered_item.Sold += item.quantity;
                   _menuService.Update(ordered_item);

                    order.Quantity += item.quantity;
                   _orderService.Update(order);
               }
            }
            SessionExtensionMethods.SetObject(HttpContext.Session, "basket", null);

            List<Order> orders = _orderService.GetByTableId(table);

            if (orders == null)
            {
                orders = new List<Order>();
            }

            List<Order> deliveredOrders = new List<Order>();
            List<Order> notDeliveredOrders = new List<Order>();

            foreach (var order in orders)
            {
                order.MenuItem = _menuService.GetById(order.ItemId);
                if (order.IsDelivered)
                    deliveredOrders.Add(order);
                else
                    notDeliveredOrders.Add(order);
            }

            OrderViewModel model = new OrderViewModel()
            {
                TableNumber = table,
                DeliveredOrders = deliveredOrders,
                NotDeliveredOrders = notDeliveredOrders
            };


            return View(model);
        }
    }
}