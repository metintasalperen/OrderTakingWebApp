using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Utilities;

namespace WebUI.Controllers
{
    public class AdminPanelController : Controller
    {
        private IMenuService _menuService;
        private IAuthService _authService;
        private IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public AdminPanelController(IMenuService menuService, IAuthService authService, IUserService userService, IWebHostEnvironment env)
        {
            _menuService = menuService;
            _authService = authService;
            _userService = userService;
            _env = env;
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public new IActionResult User(string operation = "none")
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
        [Route("/adminpanel/user/createuser")]
        public IActionResult CreateUser(UserForRegisterDto toAdd)
        {
            if (String.IsNullOrEmpty(toAdd.UserName) || String.IsNullOrEmpty(toAdd.FirstName) || String.IsNullOrEmpty(toAdd.LastName) || String.IsNullOrEmpty(toAdd.Password) || String.IsNullOrEmpty(toAdd.Role))
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
            return View("User", model);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("/adminpanel/user/updateuser")]
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
                        Action = "edit"
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
            return View("User", model);
        }
        [Authorize(Roles = Roles.Admin)]
        [Route("/adminpanel/user/edituser")]
        public IActionResult EditUser(int chosenUser)
        {
            User toEdit = _userService.GetByUserId(chosenUser);
            if (toEdit == null)
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
            return View("User", model);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("/adminpanel/user/deleteuser")]
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
            return View("User", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Menu(string category = "none")
        {
            var menu = _menuService.GetAll();
            AdminPanelMenuModel model = new AdminPanelMenuModel
            {
                Menu = menu,
                Categories = _menuService.GetCategories(),
                CurrentCategory = category
            };
            return View(model);
        }

        [Route("/adminpanel/menu/add")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("/adminpanel/menu/add")]
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Add(AdminPanelMenuAddModel addModel)
        {
            if (ModelState.IsValid)
             {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach(var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;
                            var path = Path.Combine(_env.WebRootPath, "img");
                            if (ImageUploader.UploadImage(file, path))
                            {
                                addModel.MenuItem.ImagePath = "/img/" + file.FileName;
                            }
                            else
                            {
                                _menuService.Add(addModel.MenuItem);
                                TempData.Add("message", "Item added without picture!");
                                return RedirectToAction("Menu");
                            }
                        }
                    }
                    _menuService.Add(addModel.MenuItem);
                    TempData.Add("message", "Item successfully added!");

                }
                catch (Exception ex)
                {
                    string error = ex.InnerException.Message;
                    if (error.Contains("Cannot insert duplicate key row in object"))
                        TempData.Add("message", "Insertion failed! " + addModel.MenuItem.Name + " is aready exist!");
                    else
                        TempData.Add("message", "Insertion failed! " + error);
                }
            }
            else
            {
                TempData.Add("message", "Insertion failed, invalid inputs!");
            }
            return RedirectToAction("Menu");
        }

        [Route("/adminpanel/menu/update")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int itemId)
        {
            var model = new AdminPanelMenuUpdateModel
            {
                MenuItem = _menuService.GetById(itemId)
            };
            return View(model);
        }

        [Route("/adminpanel/menu/update")]
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Update(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _menuService.Update(menuItem);
                    TempData.Add("message", "Item successfully updated!");

                } catch (Exception ex)
                {
                    string error = ex.InnerException.Message;
                    string oldName = _menuService.GetById(menuItem.ItemId).Name;
                    if (error.Contains("Cannot insert duplicate key row in object"))
                        TempData.Add("message", "Update failed! Can not rename " + oldName + " to " + menuItem.Name + ". Because " + oldName + " already exist in menu");
                    else
                        TempData.Add("message", "Update failed! " + error);
                }
            }
            else
            {
                TempData.Add("message", "Update failed, invalid inputs!");
            }
            return RedirectToAction("Menu");
        }

        [Route("/adminpanel/menu/delete")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int itemId)
        {
            try
            {
                _menuService.Delete(itemId);
                TempData.Add("message", "Item successfully deleted!");
            } catch(Exception ex)
            {
                string error = ex.InnerException.Message;
                TempData.Add("message", "Delete failed! " + error);
            }
            return RedirectToAction("Menu");
        }
    }
}