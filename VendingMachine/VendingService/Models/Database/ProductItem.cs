using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class ProductItem : BaseItem
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        public ProductItem Clone()
        {
            ProductItem item = new ProductItem();
            item.Id = this.Id;
            item.CategoryId = this.CategoryId;
            item.Name = this.Name;
            item.Price = this.Price;
            item.Image = this.Image;
            return item;
        }
    }
}
