using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Exceptions
{
    /// <summary>
    /// Specifies that the desired product is sold out
    /// </summary>
    public class SoldOutException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public SoldOutException(string message = "") : base(message)
        {

        }
    }
}
