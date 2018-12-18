using System;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingService.Database;
using VendingService.Interfaces;
using VendingService.Mock;
using VendingService.Models;
using VendingService;

namespace VendingIntegrationTests
{
    [TestClass()]
    public class DatabaseTests
    {
        //Used to begin a transaction during initialize and rollback during cleanup
        private const string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=VendingMachine;Integrated Security=true";
        private TransactionScope _tran = null;
        private IVendingService _db = null;

        private int _categoryId = BaseItem.InvalidId;
        private int _productId = BaseItem.InvalidId;
        private int _inventoryId = BaseItem.InvalidId;
        private int _vendingTransactionId = BaseItem.InvalidId;
        private int _userId1 = BaseItem.InvalidId;
        private int _userId2 = BaseItem.InvalidId;

        /// <summary>
        /// Set up the database before each test  
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _db = new VendingDBService(_connectionString);
            //_db = new MockVendingDBService();

            // Initialize a new transaction scope. This automatically begins the transaction.
            _tran = new TransactionScope();

            PasswordManager passHelper = new PasswordManager("Abcd!234");
            if (_userId1 == BaseItem.InvalidId)
            {
                var temp = new UserItem() { Id = BaseItem.InvalidId };
                temp.FirstName = "Amy";
                temp.LastName = "Rupp";
                temp.Username = "anr";
                temp.Hash = passHelper.Hash;
                temp.Salt = passHelper.Salt;
                temp.Email = "amy@tech.com";
                temp.RoleId = (int)RoleManager.eRole.Customer;

                // Add user item
                _userId1 = _db.AddUserItem(temp);
                Assert.AreNotEqual(0, _userId1);
            }

            if (_userId2 == BaseItem.InvalidId)
            {
                var temp = new UserItem() { Id = BaseItem.InvalidId };
                temp.FirstName = "Chloe";
                temp.LastName = "Rupp";
                temp.Username = "ccr";
                temp.Hash = passHelper.Hash;
                temp.Salt = passHelper.Salt;
                temp.Email = "chloe@tech.com";
                temp.RoleId = (int)RoleManager.eRole.Customer;

                // Add user item
                _userId2 = _db.AddUserItem(temp);
                Assert.AreNotEqual(0, _userId2);
            }

            if (_categoryId == BaseItem.InvalidId)
            {
                var temp = new CategoryItem() { Id = BaseItem.InvalidId };
                temp.Name = "TestCategory";
                temp.Noise = "CategoryNoise";

                // Add category item
                _categoryId = _db.AddCategoryItem(temp);
                Assert.AreNotEqual(0, _categoryId);
            }

            if (_productId == BaseItem.InvalidId)
            {
                // Add product item
                _productId = _db.AddProductItem(
                new ProductItem()
                {
                    Name = "TestProduct",
                    Price = 0.50,
                    CategoryId = _categoryId,
                    Image = "http://localhost:8080/api/food/peanuts.jpg"
                });
                Assert.AreNotEqual(0, _productId);
            }

            if (_inventoryId == BaseItem.InvalidId)
            {
                // Add inventory item
                _inventoryId = _db.AddInventoryItem(
                new InventoryItem()
                {
                    Row = 1,
                    Column = 1,
                    Qty = 4,
                    ProductId = _productId
                });
                    Assert.AreNotEqual(0, _inventoryId);
            }

            if (_vendingTransactionId == BaseItem.InvalidId)
            {
                // Add vending transaction
                _vendingTransactionId = _db.AddVendingTransaction(
                new VendingTransaction()
                {
                    Date = DateTime.UtcNow,
                    UserId = _userId1
                });
                Assert.AreNotEqual(0, _vendingTransactionId);
            }
        }

        /// <summary>
        /// Cleanup runs after every single test
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
            _categoryId = BaseItem.InvalidId;
            _productId = BaseItem.InvalidId;
            _inventoryId = BaseItem.InvalidId;
            _vendingTransactionId = BaseItem.InvalidId;
        }
        
        /// <summary>
        /// Tests the user POCO methods
        /// </summary>
        [TestMethod()]
        public void TestUser()
        {
            PasswordManager passHelper = new PasswordManager("Abcd!234");

            // Test add user
            UserItem item = new UserItem();
            item.FirstName = "Chris";
            item.LastName = "Rupp";
            item.Username = "cjr";
            item.Hash = passHelper.Hash;
            item.Salt = passHelper.Salt;
            item.Email = "chris@tech.com";
            item.RoleId = (int)RoleManager.eRole.Customer;
            int id = _db.AddUserItem(item);
            Assert.AreNotEqual(0, id);

            UserItem itemGet = _db.GetUserItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.FirstName, itemGet.FirstName);
            Assert.AreEqual(item.LastName, itemGet.LastName);
            Assert.AreEqual(item.Username, itemGet.Username);
            Assert.AreEqual(item.Hash, itemGet.Hash);
            Assert.AreEqual(item.Salt, itemGet.Salt);
            Assert.AreEqual(item.Email, itemGet.Email);

            // Test update user
            item.FirstName = "What";
            item.LastName = "What";
            item.Username = "What";
            item.Email = "What";
            item.Hash = "What";
            item.Salt = "What";
            Assert.IsTrue(_db.UpdateUserItem(item));

            itemGet = _db.GetUserItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.FirstName, itemGet.FirstName);
            Assert.AreEqual(item.LastName, itemGet.LastName);
            Assert.AreEqual(item.Username, itemGet.Username);
            Assert.AreEqual(item.Hash, itemGet.Hash);
            Assert.AreEqual(item.Salt, itemGet.Salt);
            Assert.AreEqual(item.Email, itemGet.Email);

            // Test delete user
            _db.DeleteUserItem(id);
            var users = _db.GetUserItems();
            foreach (var user in users)
            {
                Assert.AreNotEqual(id, user.Id);
            }
        }

        /// <summary>
        /// Tests the product POCO methods
        /// </summary>
        [TestMethod()]
        public void TestProduct()
        {
            // Test add product
            ProductItem item = new ProductItem();
            item.CategoryId = _categoryId;
            item.Name = "Blah";
            item.Price = 1.00;
            item.Image = "Yeah";
            int id = _db.AddProductItem(item);
            Assert.AreNotEqual(0, id);

            ProductItem itemGet = _db.GetProductItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);
            Assert.AreEqual(item.Price, itemGet.Price);
            Assert.AreEqual(item.CategoryId, itemGet.CategoryId);
            Assert.AreEqual(item.Image, itemGet.Image);

            // Test update product
            item.Name = "What";
            item.Price = 1.50;
            Assert.IsTrue(_db.UpdateProductItem(item));

            itemGet = _db.GetProductItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);
            Assert.AreEqual(item.Price, itemGet.Price);
            Assert.AreEqual(item.CategoryId, itemGet.CategoryId);
            Assert.AreEqual(item.Image, itemGet.Image);

            // Test delete product
            _db.DeleteProductItem(id);
            var products = _db.GetProductItems();
            foreach(var product in products)
            {
                Assert.AreNotEqual(id, product.Id);
            }
        }

        /// <summary>
        /// Tests the category POCO methods
        /// </summary>
        [TestMethod()]
        public void TestCategory()
        {
            // Test add category
            CategoryItem item = new CategoryItem();
            item.Name = "Blah";
            item.Noise = "Bang";
            int id = _db.AddCategoryItem(item);
            Assert.AreNotEqual(0, id);

            CategoryItem itemGet = _db.GetCategoryItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);
            Assert.AreEqual(item.Noise, itemGet.Noise);

            // Test update category
            item.Name = "What";
            item.Noise = "Kerplunk";
            Assert.IsTrue(_db.UpdateCategoryItem(item));

            itemGet = _db.GetCategoryItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Name, itemGet.Name);
            Assert.AreEqual(item.Noise, itemGet.Noise);

            // Test delete category
            _db.DeleteCategoryItem(id);
            var categories = _db.GetCategoryItems();
            foreach (var category in categories)
            {
                Assert.AreNotEqual(id, category.Id);
            }
        }

        /// <summary>
        /// Tests the inventory POCO methods
        /// </summary>
        [TestMethod()]
        public void TestInventory()
        {
            // Test add inventory
            InventoryItem item = new InventoryItem();
            item.Column = 2;
            item.Row = 3;
            item.Qty = 4;
            item.ProductId = _productId;
            int id = _db.AddInventoryItem(item);
            Assert.AreNotEqual(0, id);

            InventoryItem itemGet = _db.GetInventoryItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Column, itemGet.Column);
            Assert.AreEqual(item.Row, itemGet.Row);
            Assert.AreEqual(item.Qty, itemGet.Qty);
            Assert.AreEqual(item.ProductId, itemGet.ProductId);

            // Test update inventory
            item.Column = 3;
            item.Row = 4;
            item.Qty = 6;
            Assert.IsTrue(_db.UpdateInventoryItem(item));

            itemGet = _db.GetInventoryItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.Column, itemGet.Column);
            Assert.AreEqual(item.Row, itemGet.Row);
            Assert.AreEqual(item.Qty, itemGet.Qty);
            Assert.AreEqual(item.ProductId, itemGet.ProductId);

            // Test delete inventory
            _db.DeleteInventoryItem(id);
            var inventoryItems = _db.GetInventoryItems();
            foreach (var inventoryItem in inventoryItems)
            {
                Assert.AreNotEqual(id, inventoryItem.Id);
            }
        }

        /// <summary>
        /// Tests the vending transaction POCO methods
        /// </summary>
        [TestMethod()]
        public void TestVendingTransaction()
        {
            // Test add vending transaction
            VendingTransaction item = new VendingTransaction();
            item.Date = DateTime.UtcNow;
            item.UserId = _userId1;
            int id = _db.AddVendingTransaction(item);
            Assert.AreNotEqual(0, id);

            VendingTransaction itemGet = _db.GetVendingTransaction(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.UserId, itemGet.UserId);
            Assert.AreEqual(item.Date.ToString(), itemGet.Date.ToString());

            // Test get vending transactions
            item = new VendingTransaction();
            item.Date = DateTime.UtcNow;
            item.UserId = _userId2;
            int id2 = _db.AddVendingTransaction(item);
            Assert.AreNotEqual(0, id2);

            var vendingItems = _db.GetVendingTransactions();
            bool foundItem1 = false;
            bool foundItem2 = false;
            foreach (var vendingItem in vendingItems)
            {
                if(vendingItem.Id == id)
                {
                    foundItem1 = true;
                }
                else if(vendingItem.Id == id2)
                {
                    foundItem2 = true;
                }
            }
            Assert.IsTrue(foundItem1);
            Assert.IsTrue(foundItem2);
        }

        /// <summary>
        /// Tests the transaction item POCO methods
        /// </summary>
        [TestMethod()]
        public void TestTransactionItems()
        {
            // Test add transaction item
            TransactionItem item = new TransactionItem();
            item.ProductId = _productId;
            item.VendingTransactionId = _vendingTransactionId;
            item.SalePrice = 0.46;
            int id = _db.AddTransactionItem(item);
            Assert.AreNotEqual(0, id);

            TransactionItem itemGet = _db.GetTransactionItem(id);
            Assert.AreEqual(item.Id, itemGet.Id);
            Assert.AreEqual(item.ProductId, itemGet.ProductId);
            Assert.AreEqual(item.VendingTransactionId, itemGet.VendingTransactionId);
            Assert.AreEqual(item.SalePrice.ToString("c"), itemGet.SalePrice.ToString("c"));

            // Test get transaction items
            var transactionItems = _db.GetTransactionItems();
            bool foundItem = false;
            foreach (var transactionItem in transactionItems)
            {
                if (transactionItem.Id == id)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem);

            // Test get transaction items by vending transaction id
            transactionItems = _db.GetTransactionItems(_vendingTransactionId);
            foundItem = false;
            foreach (var transactionItem in transactionItems)
            {
                if (transactionItem.Id == id)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem);

            // Test get transaction items by year
            transactionItems = _db.GetTransactionItemsForYear(DateTime.UtcNow.Year);
            Assert.IsTrue(transactionItems.Count > 0);
        }
    }
}
