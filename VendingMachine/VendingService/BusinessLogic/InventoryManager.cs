using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Exceptions;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService
{
    /// <summary>
    /// Helper class for managing inventory
    /// </summary>
    public static class InventoryManager
    {
        /// <summary>
        /// Determines the number of rows in the vending machine
        /// </summary>
        /// <param name="items">All the vending machine items</param>
        /// <returns>The number of rows in the vending machine</returns>
        public static int RowCount(List<VendingItem> items)
        {
            HashSet<int> rows = new HashSet<int>();
            foreach(var item in items)
            {
                rows.Add(item.Inventory.Row);
            }
            return rows.Count;
        }

        /// <summary>
        /// Determines the number of columns in the vending machine
        /// </summary>
        /// <param name="items">All the vending machine items</param>
        /// <returns>The number of columns in the vending machine</returns>
        public static int ColCount(List<VendingItem> items)
        {
            HashSet<int> cols = new HashSet<int>();
            foreach (var item in items)
            {
                cols.Add(item.Inventory.Column);
            }
            return cols.Count;
        }
    }
}
