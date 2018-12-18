using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService.File
{
    public class LogFileService : ILogService
    {
        private string _folderPath = @"C:\VendingData";
        private string _fileName = @"VendingLog.txt";

        public LogFileService()
        {

        }

        public LogFileService(string folderPath)
        {
            _folderPath = folderPath;
        }

        public LogFileService(string folderPath, string fileName)
        {
            _folderPath = folderPath;
            _fileName = fileName;
        }

        public void LogOperation(VendingOperation operation)
        {
            string filePath = Path.Combine(_folderPath, _fileName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            System.IO.File.AppendAllText(filePath, operation.ToString());
        }

        public IList<VendingOperation> GetLogData()
        {
            return GetLogData(DateTime.MinValue, DateTime.MaxValue);
        }

        public IList<VendingOperation> GetLogData(DateTime startDate, DateTime endDate)
        {
            string filePath = Path.Combine(_folderPath, _fileName);
            List<VendingOperation> result = new List<VendingOperation>();

            //Open a StreamReader with the using statement
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    // Read in the line
                    string line = sr.ReadLine();
                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime date = DateTime.Parse($"{parts[0]} {parts[1]} {parts[2]}");

                    VendingOperation op = new VendingOperation();
                    op.TimeStamp = date;

                    if(parts[3] == "FEED")
                    {
                        op.OperationType = VendingOperation.eOperationType.FeedMoney;
                    }
                    else if(parts[3] == "GIVE")
                    {
                        op.OperationType = VendingOperation.eOperationType.GiveChange;
                    }
                    else
                    {
                        op.OperationType = VendingOperation.eOperationType.PurchaseItem;

                        for (int i = 3; i < parts.Length - 2; i++)
                        {
                            if(i != 3)
                            {
                                op.ProductName += " ";
                            }
                            op.ProductName += parts[i];
                        }
                    }

                    op.RunningTotal = double.Parse(parts[parts.Length - 1].Substring(1));
                    op.Price = double.Parse(parts[parts.Length - 2].Substring(1));

                    if (op.TimeStamp >= startDate && op.TimeStamp <= endDate)
                    {
                        result.Add(op);
                    }
                }
            }

            return result;
        }
    }
}
