using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class ReportItem
    {
        public string Name { get; set; }
        public int Qty { get; set; }
    }

    public class Report
    {
        Dictionary<int, int> _reportItems = new Dictionary<int, int>();
        Dictionary<int, ProductItem> _products = new Dictionary<int, ProductItem>();
    
        public Report(List<TransactionItem> items, List<ProductItem> products)
        {
            foreach (ProductItem item in products)
            {
                _products.Add(item.Id, item);
            }

            foreach (TransactionItem item in items)
            {
                TotalSales += (decimal) item.SalePrice;

                if (_reportItems.Keys.Contains(item.ProductId))
                {
                    // Increment the qty if the product already exists in the dictionary
                    _reportItems[item.ProductId]++;
                }
                else
                {
                    // If the product does not exist yet start the qty at 1
                    _reportItems[item.ProductId] = 1;
                }
            }
        }

        public decimal TotalSales { get; set; }

        public List<ReportItem> ReportItems 
        {
            get
            {
                List<ReportItem> report = new List<ReportItem>();

                foreach (var item in _reportItems)
                {
                    report.Add(new ReportItem { Name = _products[item.Key].Name, Qty = item.Value });
                }

                return report;
            }
        }
    }
}
