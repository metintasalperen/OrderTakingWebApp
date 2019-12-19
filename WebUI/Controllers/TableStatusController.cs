using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TableStatusController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ITableService _tableService;
        private readonly IHttpContextAccessor _context;

        public TableStatusController(IUserService userService, IOrderService orderService, ITableService tableService, IHttpContextAccessor context)
        {
            _userService = userService;
            _orderService = orderService;
            _tableService = tableService;
            _context = context;
        }

        public IActionResult Index()
        {
            return NotFound();
        }

        // Customer panel use this method to check table approve status
        // @returns true if customer login is approved
        // @returns false if customer login is declined or pending
        [HttpPost]
        public async Task<IActionResult> Status()
        {
            string documentContents;
            using (Stream receiveStream = _context.HttpContext.Request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = await readStream.ReadToEndAsync();
                }
            }
            RefreshModel model = JsonConvert.DeserializeObject<RefreshModel>(documentContents);
            var orders = _orderService.GetByTableId(model.TableNumber);
            
            foreach(var item in orders)
            {
                if (item.IsDummy == true)
                {
                    return Json(new { Status = "pending" });
                }
            }
            var table = _tableService.GetByTableId(model.TableNumber);
            if (table.IsApproved == true && table.IsEmpty == false)
            {
                return Json(new { Status = "approved" });
            }
            return Json(new { Status = "rejected" });
        }
    }
}