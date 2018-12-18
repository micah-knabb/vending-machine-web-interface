using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService.Mock
{
    public class MockVendingDBService : IVendingService
    {
        public MockVendingDBService()
        {
            if (_userItems.Count == 0)
            {
                TestManager.PopulateDatabaseWithUsers(this);
            }

            if (_inventoryItems.Count == 0)
            {
                TestManager.PopulateDatabaseWithInventory(this);
            }

            if (_vendingTransactions.Count == 0)
            {
                TestManager.PopulateDatabaseWithTransactions(this);
            }
        }

        #region Variables

        private static Dictionary<int, UserItem> _userItems = new Dictionary<int, UserItem>();
        private static Dictionary<int, CategoryItem> _categoryItems = new Dictionary<int, CategoryItem>();
        private static Dictionary<int, InventoryItem> _inventoryItems = new Dictionary<int, InventoryItem>();
        private static Dictionary<int, ProductItem> _productItems = new Dictionary<int, ProductItem>();
        private static Dictionary<int, VendingTransaction> _vendingTransactions = new Dictionary<int, VendingTransaction>();
        private static Dictionary<int, TransactionItem> _transactionItems = new Dictionary<int, TransactionItem>();
        private static Dictionary<int, RoleItem> _roleItems = new Dictionary<int, RoleItem>();

        private static int _userId = 1;
        private static int _categoryId = 1;
        private static int _productId = 1;
        private static int _inventoryId = 1;
        private static int _vendingTransactionId = 1;
        private static int _transactionItemId = 1;
        private static int _roleId = 1;

        #endregion

        #region Vending

        public List<VendingItem> GetVendingItems()
        {
            List<VendingItem> items = new List<VendingItem>();

            try
            {
                foreach(InventoryItem item in _inventoryItems.Values.ToList())
                {
                    VendingItem vendingItem = new VendingItem();
                    vendingItem.Inventory = item.Clone();
                    vendingItem.Product = _productItems[item.ProductId].Clone();
                    vendingItem.Category = _categoryItems[vendingItem.Product.CategoryId].Clone();
                    items.Add(vendingItem);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Mock database is corrupt. All inventory slots must contain a " +
                                    "reference to a product and all products must contain a reference to a category.", ex);
            }

            return items;
        }

        public VendingItem GetVendingItem(int row, int col)
        {
            VendingItem result = null;

            var items = GetVendingItems();
            foreach(var item in items)
            {
                if(item.Inventory.Column == col && item.Inventory.Row == row)
                {
                    result = item;
                }
            }

            return result;
        }

        #endregion

        #region Category

        public int AddCategoryItem(CategoryItem item)
        {
            item.Id = _categoryId++;
            _categoryItems.Add(item.Id, item);
            return item.Id;
        }

        public bool UpdateCategoryItem(CategoryItem item)
        {
            if(_categoryItems.ContainsKey(item.Id))
            {
                _categoryItems[item.Id] = item.Clone();
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
            return true;
        }

        public void DeleteCategoryItem(int categoryId)
        {
            if (_categoryItems.ContainsKey(categoryId))
            {
                _categoryItems.Remove(categoryId);
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
        }

        public CategoryItem GetCategoryItem(int categoryId)
        {
            CategoryItem item = null;

            if (_categoryItems.ContainsKey(categoryId))
            {
                item = _categoryItems[categoryId];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<CategoryItem> GetCategoryItems()
        {
            List<CategoryItem> items = new List<CategoryItem>();
            foreach(var item in _categoryItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        #endregion

        #region Inventory

        public int AddInventoryItem(InventoryItem item)
        {
            item.Id = _inventoryId++;
            _inventoryItems.Add(item.Id, item.Clone());
            return item.Id;
        }

        public bool UpdateInventoryItem(InventoryItem item)
        {
            if (_inventoryItems.ContainsKey(item.Id))
            {
                _inventoryItems[item.Id] = item.Clone();
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
            return true;
        }

        public void DeleteInventoryItem(int inventoryId)
        {
            if (_inventoryItems.ContainsKey(inventoryId))
            {
                _inventoryItems.Remove(inventoryId);
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
        }

        public InventoryItem GetInventoryItem(int inventoryId)
        {
            InventoryItem item = null;

            if (_inventoryItems.ContainsKey(inventoryId))
            {
                item = _inventoryItems[inventoryId];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<InventoryItem> GetInventoryItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();
            foreach (var item in _inventoryItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        #endregion

        #region Product

        public int AddProductItem(ProductItem item)
        {
            item.Id = _productId++;
            _productItems.Add(item.Id, item.Clone());
            return item.Id;
        }

        public bool UpdateProductItem(ProductItem item)
        {
            if (_productItems.ContainsKey(item.Id))
            {
                _productItems[item.Id] = item.Clone();
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
            return true;
        }

        public void DeleteProductItem(int inventoryId)
        {
            if (_productItems.ContainsKey(inventoryId))
            {
                _productItems.Remove(inventoryId);
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
        }

        public ProductItem GetProductItem(int inventoryId)
        {
            ProductItem item = null;

            if (_productItems.ContainsKey(inventoryId))
            {
                item = _productItems[inventoryId];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<ProductItem> GetProductItems()
        {
            List<ProductItem> items = new List<ProductItem>();
            foreach (var item in _productItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        #endregion

        #region VendingTransaction

        public int AddTransactionSet(VendingTransaction vendTrans, List<TransactionItem> transItems)
        {
            int vendTransId = AddVendingTransaction(vendTrans.Clone());
            foreach(var item in transItems)
            {
                TransactionItem newItem = item.Clone();
                newItem.VendingTransactionId = vendTransId;
                AddTransactionItem(newItem);
            }
            return vendTransId;
        }

        public int AddVendingTransaction(VendingTransaction item)
        {
            item.Id = _vendingTransactionId++;
            _vendingTransactions.Add(item.Id, item.Clone());
            return item.Id;
        }

        public VendingTransaction GetVendingTransaction(int id)
        {
            VendingTransaction item = null;

            if (_vendingTransactions.ContainsKey(id))
            {
                item = _vendingTransactions[id];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<VendingTransaction> GetVendingTransactions()
        {
            List<VendingTransaction> items = new List<VendingTransaction>();
            foreach (var item in _vendingTransactions)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        #endregion

        #region TransactionItem

        public int AddTransactionItem(TransactionItem item)
        {
            item.Id = _transactionItemId++;
            _transactionItems.Add(item.Id, item.Clone());
            return item.Id;
        }

        public TransactionItem GetTransactionItem(int transactionItemId)
        {
            TransactionItem item = null;

            if (_transactionItems.ContainsKey(transactionItemId))
            {
                item = _transactionItems[transactionItemId];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<TransactionItem> GetTransactionItems(int vendingTransactionId)
        {
            List<TransactionItem> items = new List<TransactionItem>();

            foreach (var item in _transactionItems.Values.ToList())
            {
                if (item.VendingTransactionId == vendingTransactionId)
                {
                    items.Add(item.Clone());
                }
            }

            return items;
        }

        public List<TransactionItem> GetTransactionItems()
        {
            List<TransactionItem> items = new List<TransactionItem>();
            foreach (var item in _transactionItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        public List<TransactionItem> GetTransactionItemsForYear(int year)
        {
            List<TransactionItem> items = new List<TransactionItem>();

            List<VendingTransaction> vendTrans = _vendingTransactions.Values.ToList();
            foreach (VendingTransaction vendItem in vendTrans)
            {
                if (vendItem.Date.Year == year)
                {
                    List<TransactionItem> transItems = GetTransactionItems(vendItem.Id);
                    foreach (var transItem in transItems)
                    {
                        items.Add(transItem.Clone());
                    }
                }
            }
            return items;
        }

        public List<TransactionItem> GetTransactionItemsForYear(int year, int userId)
        {
            List<TransactionItem> items = new List<TransactionItem>();

            List<VendingTransaction> vendTrans = _vendingTransactions.Values.ToList();
            foreach (VendingTransaction vendItem in vendTrans)
            {
                if (vendItem.Date.Year == year && vendItem.UserId == userId)
                {
                    List<TransactionItem> transItems = GetTransactionItems(vendItem.Id);
                    foreach (var transItem in transItems)
                    {
                        items.Add(transItem.Clone());
                    }
                }
            }
            return items;
        }

        #endregion

        #region UserItem

        public int AddUserItem(UserItem item)
        {
            item.Id = _userId++;
            _userItems.Add(item.Id, item.Clone());
            return item.Id;
        }

        public bool UpdateUserItem(UserItem item)
        {
            if (_userItems.ContainsKey(item.Id))
            {
                _userItems[item.Id] = item.Clone();
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
            return true;
        }

        public void DeleteUserItem(int userId)
        {
            if (_userItems.ContainsKey(userId))
            {
                _userItems.Remove(userId);
            }
            else
            {
                throw new Exception("Item does not exist.");
            }
        }

        public UserItem GetUserItem(int userId)
        {
            UserItem item = null;

            if (_userItems.ContainsKey(userId))
            {
                item = _userItems[userId];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        public List<UserItem> GetUserItems()
        {
            List<UserItem> items = new List<UserItem>();
            foreach (var item in _userItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        public UserItem GetUserItem(string username)
        {
            UserItem item = null;

            foreach (var user in _userItems)
            {
                if (user.Value.Username == username)
                {
                    item = user.Value;
                    break;
                }
            }

            if(item == null)
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();
        }

        #endregion

        #region RoleItem

        public int AddRoleItem(RoleItem item)
        {
            _roleItems.Add(item.Id, item.Clone());
            return item.Id;
        }

        public List<RoleItem> GetRoleItems()
        {
            List<RoleItem> items = new List<RoleItem>();
            foreach (var item in _roleItems)
            {
                items.Add(item.Value.Clone());
            }
            return items;
        }

        #endregion
    }
}
