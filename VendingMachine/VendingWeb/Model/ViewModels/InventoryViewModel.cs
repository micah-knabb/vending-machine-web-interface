using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendingService.Models;

namespace VendingWeb
{
    public class InventoryViewModel
    {
        public VendingItem[,] Inventory { get; } = null;

        public InventoryViewModel(int rowCount, int colCount)
        {
            Inventory = new VendingItem[colCount,rowCount];
        }

        public void AddItem(VendingItem item)
        {
            Inventory[item.Inventory.Column-1, item.Inventory.Row-1] = item;
        }
    }
}