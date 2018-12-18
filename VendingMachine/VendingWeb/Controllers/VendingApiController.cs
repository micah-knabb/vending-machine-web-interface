using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Interfaces;
using VendingService.Models;
using VendingService;

namespace VendingWeb.Controllers
{
    public class VendingApiController : VendingBaseController
    {
        public VendingApiController(IVendingService db, ILogService log) : base(db, log)
        {
        }

        [HttpGet]
        [Route("api/inventory")]
        public ActionResult GetVendingItems()
        {
            var jsonResult = Json(VendingMachine.VendingItems, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsCustomer || Role.IsServiceman || Role.IsExecutive);
        }

        [HttpGet]
        [Route("api/balance")]
        public ActionResult GetVendingBalance()
        {
            var jsonResult = Json(VendingMachine.RunningTotal, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsCustomer || Role.IsServiceman || Role.IsExecutive);
        }

        [HttpPost]
        [Route("api/feedmoney")]
        public ActionResult FeedMoney(double amount)
        {
            StatusViewModel result = null;
            try
            {
                VendingMachine.FeedMoney(amount);
                result = new StatusViewModel(eStatus.Success);
            }
            catch (Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsCustomer || Role.IsServiceman || Role.IsExecutive);
        }

        [HttpPost]
        [Route("api/purchase")]
        public ActionResult Purchase(int row, int col)
        {
            StatusViewModel result = null;
            try
            {
                var item = VendingMachine.PurchaseItem(row, col);
                result = new StatusViewModel(eStatus.Success, item.Category.Noise);
            }
            catch (Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsCustomer || Role.IsServiceman || Role.IsExecutive);
        }

        [HttpPost]
        [Route("api/change")]
        public ActionResult MakeChange()
        {
            Change change = null;
            StatusViewModel status = null;
            try
            {
                change = VendingMachine.CompleteTransaction();
                status = new StatusViewModel(eStatus.Success);
            }
            catch (Exception ex)
            {
                status = new StatusViewModel(eStatus.Error, ex.Message);
            }

            var jsonResult = Json(new ChangeViewModel(change, status), JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsCustomer || Role.IsServiceman || Role.IsExecutive);
        }

        [HttpGet]
        [Route("api/report")]
        public ActionResult GetReport(int? year, int? userId)
        {
            var jsonResult = Json(VendingMachine.GetReport(year, userId), JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsExecutive);
        }

        [HttpGet]
        [Route("api/log")]
        public ActionResult GetLog(DateTime? startDate, DateTime? endDate)
        {
            var jsonResult = Json(VendingMachine.GetLog(startDate, endDate), JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsExecutive);
        }

        [HttpGet]
        [Route("api/users")]
        public ActionResult GetUsers()
        {
            var jsonResult = Json(VendingMachine.Users, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult, Role.IsAdministrator);
        }
    }
}