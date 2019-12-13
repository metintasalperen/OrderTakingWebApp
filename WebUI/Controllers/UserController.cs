using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Entities.Concrete;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser(UserForRegisterDto toAdd)
        {
            var result = _authService.Register(toAdd, toAdd.Password);
            var waiters = _userService.GetByRole("Waiter");
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "create"
            };
            if (!result.Success)
            {
                TempData.Add("createMessage", result.Message);
                return View("Index", model);
            }
            return View("Index", model);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int chosenUser)
        {

            
            var waiters = _userService.GetByRole("Waiter");
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "none"
            };
            return View("Index", model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int chosenUser)
        {
            var entity = _userService.GetByUserId(chosenUser);
            if(entity != null)
            {
                _userService.Delete(chosenUser);
            }
            var waiters = _userService.GetByRole("Waiter");
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "delete"
            };
            return View("Index", model);
        }
    }
}