using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Dtos;
using Entities.Concrete;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Core.Utilities.Hashing;

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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index(string operation = "none")
        {
            var waiters = _userService.GetByRole(Roles.Waiter);
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = operation
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult CreateUser(UserForRegisterDto toAdd)
        {
            if (String.IsNullOrEmpty(toAdd.UserName) ||String.IsNullOrEmpty(toAdd.FirstName) || String.IsNullOrEmpty(toAdd.LastName) || String.IsNullOrEmpty(toAdd.Password)|| String.IsNullOrEmpty(toAdd.Role))
            {
                UserViewModel blankFields = new UserViewModel
                {
                    Action = "create"
                };
                TempData.Add("createMessage", "Please provide all info!");
                return View("Index", blankFields);
            }
            var result = _authService.Register(toAdd, toAdd.Password);
            var waiters = _userService.GetByRole(Roles.Waiter);
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "create"
            };
            if (!result.Success)
            {
                TempData.Add("createMessage", result.Message);
            }
            return View("Index", model);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpdateUser(UserForRegisterDto toUpdate, int chosenUser)
        {
            var exist = _userService.GetByUserId(chosenUser);
            if (exist != null)
            {
                if (String.IsNullOrEmpty(toUpdate.FirstName) || String.IsNullOrEmpty(toUpdate.LastName) || String.IsNullOrEmpty(toUpdate.Password))
                {
                    UserViewModel blankFields = new UserViewModel
                    {
                        ChosenUser = exist,
                        Action =  "edit"
                    };
                    TempData.Add("updateMessage", "Please provide all info!");
                    return View("Index", blankFields);
                }
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(toUpdate.Password, out passwordHash, out passwordSalt);
                exist.FirstName = toUpdate.FirstName;
                exist.LastName = toUpdate.LastName;
                exist.PasswordHash = passwordHash;
                exist.PasswordSalt = passwordSalt;
                exist.PhoneNumber = toUpdate.PhoneNumber;
                exist.Role = toUpdate.Role;
                _userService.Update(exist);
            }
            var waiters = _userService.GetByRole(Roles.Waiter);
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "list"
            };
            return View("Index", model);
        }
        [Authorize(Roles = Roles.Admin)]
        public IActionResult EditUser(int chosenUser)
        {
            User toEdit = _userService.GetByUserId(chosenUser);
            if(toEdit == null)
            {
                var waiters = _userService.GetByRole(Roles.Waiter);
                TempData.Add("editMessage", "Please choose user!");
                UserViewModel list = new UserViewModel
                {
                    Users = waiters,
                    Action = "list"
                };
                return View("Index", list);
            }
            UserViewModel model = new UserViewModel
            {
                ChosenUser = toEdit,
                Action = "edit"
            };
            return View("Index", model);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteUser(int chosenUser)
        {
            var entity = _userService.GetByUserId(chosenUser);
            if (entity != null)
            {
                _userService.Delete(chosenUser);
            }
            else
            {
                TempData.Add("deleteMessage", "Please choose user!");
            }
            var waiters = _userService.GetByRole(Roles.Waiter);
            UserViewModel model = new UserViewModel
            {
                Users = waiters,
                Action = "delete"
            };
            return View("Index", model);
        }
    }
}