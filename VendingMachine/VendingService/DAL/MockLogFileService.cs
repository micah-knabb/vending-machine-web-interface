using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingService.Mock
{
    public class MockLogFileService : ILogService
    {
        private List<VendingOperation> _operations = new List<VendingOperation>();

        public void LogOperation(VendingOperation operation)
        {
            _operations.Add(operation.Clone());
        }

        public IList<VendingOperation> GetLogData(DateTime startDate, DateTime endDate)
        {
            List<VendingOperation> result = new List<VendingOperation>();
            foreach(var item in _operations)
            {
                if(item.TimeStamp >= startDate && item.TimeStamp <= endDate)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public IList<VendingOperation> GetLogData()
        {
            return _operations;
        }
    }
}
