using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TableController : Controller
    {
        private ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }
        public IActionResult Index(string operation = "none")
        {
            var tables = _tableService.GetAll();
            TableViewModel model = new TableViewModel
            {
                Tables= tables,
                Action = operation
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateTable(Table toAdd)
        {
            _tableService.Add(toAdd);
            var tables = _tableService.GetAll();
            TableViewModel model = new TableViewModel
            {
                Tables = tables,
                Action = "create"
            };
            TempData.Add("createMessage", "Table Created!");
            return View("Index", model);
        }
        [HttpPost]
        public IActionResult DeleteTable(int chosenTable)
        {
            var entity = _tableService.GetByTableId(chosenTable);
            if (entity != null)
            {
                _tableService.Delete(chosenTable);
            }
            var tables = _tableService.GetAll();
            TableViewModel model = new TableViewModel
            {
                Tables = tables,
                Action = "delete"
            };
            return View("Index", model);
        }
    }
}