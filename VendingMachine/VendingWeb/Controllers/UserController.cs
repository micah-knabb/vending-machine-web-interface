using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Interfaces;
using VendingService;
using VendingService.Models;
using VendingService.Exceptions;

namespace VendingWeb.Controllers
{
    public class UserController : VendingBaseController
    {
        public UserController(IVendingService db, ILogService log) : base(db, log)
        {
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (VendingMachine.IsAuthenticated)
            {
                VendingMachine.LogoutUser();
            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                VendingMachine.LoginUser(model.Username, model.Password);

                result = RedirectToAction("Index", "Vending");
            }
            catch (Exception)
            {
                result = View("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (VendingMachine.IsAuthenticated)
            {
                VendingMachine.LogoutUser();
            }

            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                try
                {
                    User userModel = new User();
                    userModel.ConfirmPassword = model.ConfirmPassword;
                    userModel.Password = model.Password;
                    userModel.FirstName = model.FirstName;
                    userModel.LastName = model.LastName;
                    userModel.Username = model.Username;
                    userModel.Email = model.Email;
                    VendingMachine.RegisterUser(userModel);
                }
                catch(UserExistsException)
                {
                    ModelState.AddModelError("invalid-user", "The username is already taken.");
                    throw;
                }                
                
                result = RedirectToAction("Index", "Vending");
            }
            catch (Exception)
            {
                result = View("Register");
            }

            return result;
        }
    }
}