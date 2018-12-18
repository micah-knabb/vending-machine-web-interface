using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class TransactionItem : BaseItem
    {
        public int VendingTransactionId { get; set; }
        public int ProductId { get; set; }
        public double SalePrice { get; set; }

        public TransactionItem Clone()
        {
            TransactionItem item = new TransactionItem();
            item.Id = this.Id;
            item.ProductId = this.ProductId;
            item.SalePrice = this.SalePrice;
            item.VendingTransactionId = this.VendingTransactionId;
            return item;
        }
    }
}
