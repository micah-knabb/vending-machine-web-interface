using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Models;

namespace VendingService.Interfaces
{
    public interface ILogService
    {
        void LogOperation(VendingOperation operation);
        IList<VendingOperation> GetLogData(DateTime startDate, DateTime endDate);
        IList<VendingOperation> GetLogData();
    }
}
