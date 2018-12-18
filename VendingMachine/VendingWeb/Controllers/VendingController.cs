using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService;
using VendingService.Interfaces;

namespace VendingWeb.Controllers
{
    public class VendingController : VendingBaseController
    {
        public VendingController(IVendingService db, ILogService log) : base(db,log)
        {
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            string nextView = "Index";

            if (Role.IsAdministrator)
            {
                nextView = "Admin";
            }

            return GetAuthenticatedView(nextView);
        }
                
        [HttpGet]
        public ActionResult Log()
        {
            if (!Role.IsExecutive)
            {
                VendingMachine.LogoutUser();
            }

            return GetAuthenticatedView("Log");
        }

        [HttpGet]
        public ActionResult Report()
        {
            if (!Role.IsExecutive)
            {
                VendingMachine.LogoutUser();
            }

            return GetAuthenticatedView("Report", VendingMachine.Users);
        }

        [HttpGet]
        public ActionResult About()
        {
            return GetAuthenticatedView("About");
        }

        [HttpGet]
        public ActionResult Modify()
        {
            if (!Role.IsServiceman)
            {
                VendingMachine.LogoutUser();
            }

            return GetAuthenticatedView("ModifyInventory");
        }

        [HttpGet]
        public ActionResult Admin()
        {
            if (!Role.IsAdministrator)
            {
                VendingMachine.LogoutUser();
            }

            return GetAuthenticatedView("Admin");
        }
    }
}