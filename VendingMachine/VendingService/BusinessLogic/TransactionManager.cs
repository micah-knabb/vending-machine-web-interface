using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService
{
    public class TransactionManager
    {
        public enum eTransactionState
        {
            Unknown = 0,
            FeedMoney = 1,
            Purchase = 2,
            Change = 3
        }

        private Change _change = new Change();
        private eTransactionState _state = eTransactionState.Unknown;
        private IVendingService _db = null;
        private ILogService _log = null;
        private List<TransactionItem> _transactionItems = new List<TransactionItem>();
        private double _runningTotal = 0.0;

        public double RunningTotal
        {
            get
            {
                return _runningTotal;
            }
        }

        public Change LastChange
        {
            get
            {
                return _change;
            }
        }

        public eTransactionState LastState
        {
            get
            {
                return _state;
            }
        }

        public TransactionManager(IVendingService db, ILogService log)
        {
            _db = db;
            _log = log;
        }

        public double AddPurchaseTransaction(int productId)
        {
            _state = eTransactionState.Purchase;

            ProductItem product = _db.GetProductItem(productId);

            if (product.Price > _runningTotal)
            {
                throw new Exception("More money is required.");
            }            

            TransactionItem transactionItem = new TransactionItem();
            transactionItem.ProductId = productId;
            transactionItem.SalePrice = product.Price;
            _transactionItems.Add(transactionItem);

            VendingOperation operation = new VendingOperation();
            operation.OperationType = VendingOperation.eOperationType.PurchaseItem;
            operation.Price = product.Price;
            operation.RunningTotal = _runningTotal;
            operation.ProductName = product.Name;
            _log.LogOperation(operation);

            _runningTotal -= product.Price;

            return _runningTotal;
        }

        public double AddFeedMoneyOperation(double amountAdded)
        {
            _state = eTransactionState.FeedMoney;

            VendingOperation operation = new VendingOperation();
            operation.OperationType = VendingOperation.eOperationType.FeedMoney;
            operation.Price = amountAdded;
            operation.RunningTotal = _runningTotal;
            _log.LogOperation(operation);

            _runningTotal += amountAdded;

            return _runningTotal;
        }

        public Change AddGiveChangeOperation()
        {
            _state = eTransactionState.Change;

            double result = _runningTotal;
            VendingOperation operation = new VendingOperation();
            operation.OperationType = VendingOperation.eOperationType.GiveChange;
            operation.RunningTotal = _runningTotal;
            _log.LogOperation(operation);

            _change = GetChange();

            _runningTotal = 0.0;

            return _change;
        }

        public void CommitTransaction(int userId)
        {
            VendingTransaction vendTrans = new VendingTransaction();
            vendTrans.Date = DateTime.UtcNow;
            vendTrans.UserId = userId;

            _db.AddTransactionSet(vendTrans, _transactionItems);
        }

        private Change GetChange()
        {
            Change result = new Change();

            int temp = (int)(_runningTotal * 100.0);
            result.Dollars = temp / 100;
            temp -= result.Dollars * 100;

            result.Quarters = temp / 25;
            temp -= result.Quarters * 25;

            result.Dimes = temp / 10;
            temp -= result.Dimes * 10;

            result.Nickels = temp / 5;
            temp -= result.Nickels * 5;

            result.Pennies = temp;

            return result;
        }
    }
}
