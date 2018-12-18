using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Database;
using VendingService.Exceptions;
using VendingService.File;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService
{
    /// <summary>
    /// Manages all the business logic and data for the Vending Machine
    /// </summary>
    public class VendingMachine
    {
        /// <summary>
        /// Manages the report
        /// </summary>
        private ReportManager _repMgr = null;

        /// <summary>
        /// Manages transactions
        /// </summary>
        private TransactionManager _transMgr = null;

        /// <summary>
        /// Manages the user authentication and authorization
        /// </summary>
        private RoleManager _roleMgr = null;

        /// <summary>
        /// The data access layer for the vending machine
        /// </summary>
        private IVendingService _db = null;

        /// <summary>
        /// Use this to write and read from the log file
        /// </summary>
        private ILogService _log = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="log"></param>
        public VendingMachine(IVendingService db, ILogService log)
        {
            _db = db;
            _log = log;
            _transMgr = new TransactionManager(_db, _log);
            _repMgr = new ReportManager(_db);;
            _roleMgr = new RoleManager(null);
        }

        public bool IsAuthenticated
        {
            get
            {
                return _roleMgr.User != null;
            }
        }

        public void RegisterUser(User userModel)
        {
            UserItem userItem = null;
            try
            {
                userItem = _db.GetUserItem(userModel.Username);
            }
            catch (Exception)
            {
            }

            if (userItem != null)
            {
                throw new UserExistsException("The username is already taken.");
            }

            if (userModel.Password != userModel.ConfirmPassword)
            {
                throw new PasswordMatchException("The password and confirm password do not match.");
            }

            PasswordManager passHelper = new PasswordManager(userModel.Password);
            UserItem newUser = new UserItem()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username,
                Salt = passHelper.Salt,
                Hash = passHelper.Hash,
                RoleId = (int)RoleManager.eRole.Customer
            };

            _db.AddUserItem(newUser);
            LoginUser(newUser.Username, userModel.Password);
        }

        public void LoginUser(string username, string password)
        {
            UserItem user = null;

            try
            {
                user = _db.GetUserItem(username);
            }
            catch (Exception)
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            PasswordManager passHelper = new PasswordManager(password, user.Salt);
            if (!passHelper.Verify(user.Hash))
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            _roleMgr = new RoleManager(user);
        }

        public void LogoutUser()
        {
            _roleMgr = new RoleManager(null);
        }

        public IList<UserItem> Users
        {
            get
            {
                return _db.GetUserItems();
            }
        }

        public VendingItem[,] VendingItems
        {
            get
            {
                var items = _db.GetVendingItems();
                var result = new VendingItem[InventoryManager.ColCount(items), InventoryManager.RowCount(items)];

                foreach(var item in items)
                { 
                    result[item.Inventory.Column - 1, item.Inventory.Row - 1] = item;
                }

                return result;
            }
        }

        public RoleManager Role
        {
            get
            {
                return _roleMgr;
            }
        }

        public double RunningTotal
        {
            get
            {
                return _transMgr.RunningTotal;
            }
        }

        public void FeedMoney(double amt)
        {
            _transMgr.AddFeedMoneyOperation(amt);
        }

        /// <summary>
        /// Reduces the qty of the vending item by 1 if there is at least 1 item left in the slot
        /// </summary>
        /// <param name="row">The 1 based row for the item</param>
        /// <param name="col">The 1 based col for the item</param>
        public VendingItem PurchaseItem(int row, int col)
        {
            var item = _db.GetVendingItem(row, col);

            if (item.Product.Price > RunningTotal)
            {
                throw new InsufficientFundsException("The vending machine does not have enough funds for this purchase.");
            }

            if (item.Inventory.Qty == 0)
            {
                throw new SoldOutException("Product is sold out.");
            }
            else
            {
                item.Inventory.Qty--;
                _db.UpdateInventoryItem(item.Inventory);
            }
                        
            _transMgr.AddPurchaseTransaction(item.Product.Id);

            return item;
        }

        public Change CompleteTransaction()
        {
            var change = _transMgr.AddGiveChangeOperation();
            _transMgr.CommitTransaction(CurrentUser.Id);
            return change;
        }

        public UserItem CurrentUser
        {
            get
            {
                return _roleMgr.User;
            }
        }

        public Report GetReport(int? year, int? userId)
        {
            Report report = null;

            if (year == null)
            {
                year = DateTime.Now.Year;
            }

            if (userId == null)
            {
                report = _repMgr.GetReport((int)year, _db.GetProductItems());
            }
            else
            {
                report = _repMgr.GetReport((int)year, _db.GetProductItems(), (int)userId);
            }

            return report;
        }

        public IList<VendingOperation> GetLog(DateTime? startDate, DateTime? endDate)
        {            
            IList<VendingOperation> result = null;

            if (startDate != null && endDate != null)
            {
                result = _log.GetLogData((DateTime)startDate, (DateTime)endDate);
            }
            else
            {
                result = _log.GetLogData();
            }

            return result;
        }
    }
}
