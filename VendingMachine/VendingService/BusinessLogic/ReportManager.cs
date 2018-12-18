using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Database;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService
{
    public class ReportManager
    {
        private IVendingService _db;

        public ReportManager(IVendingService db)
        {
            _db = db;
        }

        public Report GetReport(int year, List<ProductItem> products)
        {
            var transItems = _db.GetTransactionItemsForYear(year);
            Report report = new Report(transItems, products);
            return report;
        }

        public Report GetReport(int year, List<ProductItem> products, int userId)
        {
            var transItems = _db.GetTransactionItemsForYear(year, userId);
            Report report = new Report(transItems, products);
            return report;
        }
    }
}
