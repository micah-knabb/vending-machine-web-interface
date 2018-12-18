using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Models;

namespace VendingWeb
{
    public class ChangeViewModel
    {
        public Change Change { get; }
        public StatusViewModel Status { get; }

        public ChangeViewModel(Change change, StatusViewModel status)
        {
            Change = change;
            Status = status;
        }
    }
}
