using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Models
{
    public class Change
    {
        public int Dollars { get; set; } = 0;
        public int Quarters { get; set; } = 0;
        public int Dimes { get; set; } = 0;
        public int Nickels { get; set; } = 0;
        public int Pennies { get; set; } = 0;        
    }
}
