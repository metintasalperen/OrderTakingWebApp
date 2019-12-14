using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        private IMenuService _menuService;
        public OrderController(IOrderService orderService, IMenuService menuService)
        {
            _orderService = orderService;
            _menuService = menuService;
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
    }
}