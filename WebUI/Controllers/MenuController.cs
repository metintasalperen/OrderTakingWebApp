using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService _menuService;
        public IActionResult Index()
        {
            return View();
        }
    }
}