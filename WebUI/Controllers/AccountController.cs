using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
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
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var userRole = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Role)?.Value;
                if (userRole == Roles.Waiter)
                {
                    return RedirectToAction("Index", "WaiterPanel");
                }
                else if (userRole == Roles.Admin)
                {
                    return RedirectToAction("Index", "AdminPanel");
                }
            }
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
                TempData.Add("loginMessage", result.Message);
                return View();
            }

            HttpContext.Session.SetString("JWToken", result.Data.Token);

            return userToLogin.Data.Role == Roles.Admin ? RedirectToAction("Index", "AdminPanel") :
                RedirectToAction("Index", "WaiterPanel");
        }
    }
}