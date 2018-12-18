using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class VendingTransaction : BaseItem
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public VendingTransaction Clone()
        {
            VendingTransaction item = new VendingTransaction();
            item.Id = this.Id;
            item.Date = this.Date;
            item.UserId = this.UserId;
            return item;
        }
    }
}
