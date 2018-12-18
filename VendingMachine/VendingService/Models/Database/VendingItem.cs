using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class VendingItem
    {
        public InventoryItem Inventory { get; set; } = new InventoryItem();
        public ProductItem Product { get; set; } = new ProductItem();
        public CategoryItem Category { get; set; } = new CategoryItem();

        public override string ToString()
        {
            return $"[{Inventory.Row},{Inventory.Column}] Qty:{Inventory.Qty} Product:{Product.Name} Category:{Category.Name}";
        }

        public VendingItem Clone()
        {
            VendingItem item = new VendingItem();
            item.Inventory = this.Inventory.Clone();
            item.Category = this.Category.Clone();
            item.Product = this.Product.Clone();
            return item;
        }
    }
}
