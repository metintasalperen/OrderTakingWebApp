using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private IAuthService _authService;
        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
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
        public IActionResult Index(UserForRegisterDto toAdd)
        {
            _authService.Register(toAdd, toAdd.Password);
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