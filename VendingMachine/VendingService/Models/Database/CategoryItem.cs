using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class CategoryItem : BaseItem
    {
        public string Name { get; set; }
        public string Noise { get; set; }

        public CategoryItem Clone()
        {
            CategoryItem item = new CategoryItem();
            item.Id = this.Id;
            item.Name = this.Name;
            item.Noise = this.Noise;
            return item;
        }
    }
}
