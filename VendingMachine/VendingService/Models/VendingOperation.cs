using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class VendingOperation
    {
        public enum eOperationType
        {
            Invalid = 0,
            FeedMoney = 1,
            GiveChange = 2,
            PurchaseItem = 3
        }

        public DateTime TimeStamp { get; set; }
        public string TimeStampStr
        {
            get
            {
                return TimeStamp.ToLocalTime().ToString();
            }
        }
        public eOperationType OperationType { get; set; } = eOperationType.Invalid;
        public double Price { get; set; } = 0.0;
        public double RunningTotal { get; set; } = 0.0;
        public string ProductName { get; set; } = "";
        

        public VendingOperation()
        {
            TimeStamp = DateTime.UtcNow;
        }

        private string GetOperationString()
        {
            string result = "";

            switch(OperationType)
            {
                case eOperationType.FeedMoney:
                    result = "FEED MONEY:";
                    break;
                
                case eOperationType.GiveChange:
                    result = "GIVE CHANGE:";
                    break;

                case eOperationType.PurchaseItem:
                    result = ProductName;
                    break;

                default:
                    throw new Exception("Unknown operation type.");
            }

            return result;
        }

        public override string ToString()
        {
            double leftPrice = 0.0;
            double rightPrice = 0.0;

            if (OperationType == eOperationType.FeedMoney)
            {
                leftPrice = Price;
                rightPrice = RunningTotal;
            }
            else if (OperationType == eOperationType.PurchaseItem)
            {
                leftPrice = RunningTotal;
                rightPrice = RunningTotal - Price;
            }
            else if (OperationType == eOperationType.GiveChange)
            {
                leftPrice = RunningTotal;
            }
            else
            {
                throw new Exception("Unknown operation type.");
            }

            string result = $"{TimeStamp.ToLocalTime().ToString().PadRight(25, ' ')}" +
                            $"{GetOperationString().PadRight(35, ' ')}" +
                            $"{leftPrice.ToString("c").PadRight(10, ' ')}" +
                            $"{rightPrice.ToString("c")}\r\n";

            return result;
        }

        public VendingOperation Clone()
        {
            VendingOperation operation = new VendingOperation();
            operation.OperationType = this.OperationType;
            operation.Price = this.Price;
            operation.ProductName = this.ProductName;
            operation.RunningTotal = this.RunningTotal;
            operation.TimeStamp = this.TimeStamp;

            return operation;
        }
    }
}
