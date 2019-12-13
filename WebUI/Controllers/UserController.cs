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
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index(string operation = "none")
        {
            var waiters = _userService.GetByRole("Waiter");
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = operation
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(User toAdd)
        {
            // Implement hash and salt operations
            /*
            int intValue = 5;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] result = intBytes;
            toAdd.PasswordHash = result;
            toAdd.PasswordSalt = result;
            */
            _userService.Add(toAdd);
            var waiters = _userService.GetByRole("Waiter");
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "none"
            };
            return View(model);
        }
    }
}