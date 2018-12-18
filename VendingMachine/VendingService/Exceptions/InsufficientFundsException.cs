using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Exceptions
{
    /// <summary>
    /// Specifies that there are insufficient funds for the intended purchase
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public InsufficientFundsException(string message = "") : base(message)
        {

        }
    }
}
