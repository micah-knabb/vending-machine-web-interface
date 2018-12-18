using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingService.Database;
using VendingService;
using VendingService.Interfaces;
using VendingService.Mock;
using VendingService.Models;

namespace VendingIntegrationTests
{
    [TestClass]
    public class LoadDatabase
    {
        private const string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=VendingMachine;Integrated Security=true";

        //[TestMethod]
        public void PopulateDatabase()
        {
            IVendingService db = new VendingDBService(_connectionString);
            //IVendingService db = new MockVendingDBService();

            TestManager.PopulateDatabaseWithUsers(db);
            TestManager.PopulateDatabaseWithInventory(db);
            TestManager.PopulateDatabaseWithTransactions(db);
        }
    }
}
