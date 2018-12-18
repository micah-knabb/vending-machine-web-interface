using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingService.Exceptions
{
    /// <summary>
    /// Specifies that the desired product is not available
    /// </summary>
    public class InvalidProductSelection : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public InvalidProductSelection(string message = "") : base(message)
        {

        }
    }
}
