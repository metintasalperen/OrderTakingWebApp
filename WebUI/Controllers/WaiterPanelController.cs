using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public WaiterPanelController(IOrderService orderService, IUserService userService, IMenuService menuService, ITableService tableService, IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _menuService = menuService;
            _userService = userService;
        }
        public IActionResult Index(int table = 0)
        {
            var allOrders = _orderService.GetAll();
            var dummyOrders = new List<Order>();
            var orders = new List<Order>();
            foreach(var item in allOrders)
            {
                if (item.IsDummy && !item.IsDelivered)
                {
                    dummyOrders.Add(item);
                }
                else
                {
                    orders.Add(item);
                }
            }
            var allTableOrders = _orderService.GetByTableId(table);
            var tableOrders = new List<Order>();
            foreach(var item in allTableOrders)
            {
                if (!item.IsDummy)
                {
                    tableOrders.Add(item);
                }
            }
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
                CurrentTable = table,
                TableOrders = tableOrders.ToList(),
                WaiterId = Int32.Parse(userId),
                DummyOrders = dummyOrders,
            };
            return View(model);
        }
        public IActionResult MakeDelivered(int orderId, int table)
        {
            var entity = _orderService.GetByOrderId(orderId);
            var allEntities = _orderService.GetAll();
            
            IEnumerable<Order> sameMenuItemWithDelivered = //query variable
                 from order in allEntities //required
                 where order.ItemId == entity.ItemId && order.TableId == entity.TableId && order.IsDelivered == true && order.IsDummy == false
                 select order; //must end with select or group

            List<Order> sameMenuItemWithDeliveredList = sameMenuItemWithDelivered.ToList();
            if (entity != null)
            {
                if(!sameMenuItemWithDeliveredList.Any())
                {
                    entity.IsDelivered = true;
                    _orderService.Update(entity);
                }
                else
                {
                    sameMenuItemWithDeliveredList[0].Quantity += entity.Quantity;
                    _orderService.Update(sameMenuItemWithDeliveredList[0]);
                    _orderService.Delete(entity.OrderId);
                }

            }

            return RedirectToAction("Index");
        }
        public IActionResult MakeAvailable(int tableId)
        {
            var orders = _orderService.GetAll();
            var tableOrders = _orderService.GetByTableId(tableId);
            var menus = _menuService.GetAll();
            var tables = _tableService.GetAll();
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = orders.ToList(),
                Menu = menus.ToList(),
                Table = tables.ToList(),
                CurrentTable = tableId,
                TableOrders = tableOrders.ToList(),
                WaiterId = Int32.Parse(userId)
            };
            return View("Payment", model);
        }
        public IActionResult BackToIndex(int tableId,int table = 0)
        {
            var tableEntity = _tableService.GetByTableId(tableId);
            if (tableEntity != null)
            {
                tableEntity.IsEmpty = true;
                tableEntity.Token = null;
                tableEntity.IsApproved = false;
                _tableService.Update(tableEntity);
            }
            var orderEntities = _orderService.GetByTableId(tableId);
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var waiter = _userService.GetByUserId(Int32.Parse(userId));
            waiter.CountOfTable--;
            _userService.Update(waiter);

            foreach (var item in orderEntities)
            {
                _orderService.Delete(item.OrderId);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ApproveRequest(int orderId)
        {
            var dummyOrder = _orderService.GetByOrderId(orderId);
            var tableId = dummyOrder.TableId;
            var table = _tableService.GetByTableId(tableId);
            table.IsApproved = true;
            _tableService.Update(table);

            dummyOrder.IsDelivered = true;
            _orderService.Update(dummyOrder);
            //_orderService.Delete(orderId);
            return RedirectToAction("Index");
        }

        public IActionResult DeclineRequest(int orderId)
        {
            var dummyOrder = _orderService.GetByOrderId(orderId);
            var tableId = dummyOrder.TableId;
            var table = _tableService.GetByTableId(tableId);
            var waiterId = dummyOrder.WaiterId;
            var waiter = _userService.GetByUserId(waiterId);
            waiter.CountOfTable--;
            _userService.Update(waiter);
            table.IsEmpty = true;
            table.Token = null;
            _tableService.Update(table);

            _orderService.Delete(orderId);
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            var result = _authService.LogOut(_session);
            return Redirect("~/account/login");
        }
    }
}
