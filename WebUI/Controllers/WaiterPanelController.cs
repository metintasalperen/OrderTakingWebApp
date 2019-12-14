using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{

    [Authorize(Roles = Roles.Waiter)]
    public class WaiterPanelController : Controller
    {
        private IOrderService _orderService;
        private IUserService _userService;
        private IMenuService _menuService;
        private ITableService _tableService;
        public WaiterPanelController(IOrderService orderService, IUserService userService, IMenuService menuService, ITableService tableService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _menuService = menuService;
            _userService = userService;
        }
        public IActionResult Index(int table = 0)
        {
            var orders = _orderService.GetAll();
            var tableOrders = _orderService.GetByTableId(table);
            var menus = _menuService.GetAll();
            var tables = _tableService.GetAll();

            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            if (string.IsNullOrEmpty(userId))
            {
                return View("Error");
            }
            // loginden gelecek username ile usere sorgu atılacak, dönen waiterin idsi waiterid olarak modele yollanacak.
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = orders.ToList(),
                Menu = menus.ToList(),
                Table = tables.ToList(),
                CurrentCategory = HttpContext.Request.Query["tableid"],
                TableOrders = tableOrders.ToList(),
                WaiterId = Int32.Parse(userId)
            };
            return View(model);
        }
        public IActionResult MakeDelivered(int orderId, int table)
        {
            var entity = _orderService.GetByOrderId(orderId);
            if (entity != null)
            {
                entity.IsDelivered = true;
                _orderService.Update(entity);
            }

            var orders = _orderService.GetAll();
            var tableOrders = _orderService.GetByTableId(table);
            var menus = _menuService.GetAll();
            var tables = _tableService.GetAll();

            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = orders.ToList(),
                Menu = menus.ToList(),
                Table = tables.ToList(),
                CurrentCategory = HttpContext.Request.Query["tableid"],
                TableOrders = tableOrders.ToList(),
                WaiterId = Int32.Parse(userId)
            };
            return View("Index", model);
        }
        public IActionResult MakeAvailable(int tableId, int table)
        {
            var tableEntity = _tableService.GetByTableId(tableId);
            if (tableEntity != null)
            {
                tableEntity.IsEmpty = true;
                _tableService.Update(tableEntity);
            }
            var orderEntities = _orderService.GetByTableId(tableId);

            foreach (var item in orderEntities)
            {
                _orderService.Delete(item.OrderId);
            }

            var orders = _orderService.GetAll();
            var tableOrders = _orderService.GetByTableId(table);
            var menus = _menuService.GetAll();
            var tables = _tableService.GetAll();
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = orders.ToList(),
                Menu = menus.ToList(),
                Table = tables.ToList(),
                CurrentCategory = HttpContext.Request.Query["tableid"],
                TableOrders = tableOrders.ToList(),
                WaiterId = Int32.Parse(userId)
            };
            return View("Index", model);
        }
    }
}
