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
    public class AccountController : Controller
    {
        private IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var loginDto = new UserForLoginDto
            {
                UserName = loginViewModel.Username,
                Password = loginViewModel.Password
            };
            var userToLogin = _authService.Login(loginDto);
            if (!userToLogin.Success)
            {
                TempData.Add("loginMessage", userToLogin.Message);
                return View();
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (!result.Success)
            {
                //Will change
                return View();
            }
            return View();
        }
    }
}