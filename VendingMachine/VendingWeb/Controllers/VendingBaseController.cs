using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingWeb.Controllers
{
    public class VendingBaseController : Controller
    {
        public const string VendingMachineKey = "VendingMachine";
        
        private IVendingService _db;
        private ILogService _log;

        public VendingBaseController(IVendingService db, ILogService log)
        {
            _db = db;
            _log = log;
        }

        public ActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            ActionResult result = null;
            if (VendingMachine.IsAuthenticated)
            {
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }
            return result;
        }

        public JsonResult GetAuthenticatedJson(JsonResult json, bool hasPermission)
        {
            JsonResult result = null;
            if (!hasPermission && VendingMachine.IsAuthenticated)
            {
                result = Json(new { error = "User is not permitted to access this data." }, JsonRequestBehavior.AllowGet);
            }
            else if (VendingMachine.IsAuthenticated)
            {
                result = json;
            }
            else
            {
                result = Json(new { error = "User is not authenticated." }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public VendingMachine VendingMachine
        {
            get
            {
                VendingMachine vm = Session[VendingMachineKey] as VendingMachine;

                if (vm == null)
                {
                    vm = new VendingMachine(_db, _log);
                    Session[VendingMachineKey] = vm;
                }

                return vm;
            }
        }

        public RoleManager Role
        {
            get
            {
                return VendingMachine.Role;
            }
        }
    }
}