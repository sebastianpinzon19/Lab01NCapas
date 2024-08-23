using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class CustomerExceptions : Exception
    {
        // You can add more static methods here to throw other customer-related exceptions

        public CustomerExceptions()
        {
            throw new CustomerExceptions($"No customers found in the database.");
        }

        private CustomerExceptions(string message)
            : base(message)
        {
            throw new Exception(message);
        }

        public static void ThrowCustomerAlreadyExistsException(string firstName, string lastName)
        {
            throw new CustomerExceptions($"A client with the name already exists: {firstName} {lastName}.");
        }

        public static void ThrowInvalidCustomerIdException(int id)
        {
            throw new CustomerExceptions($"Invalid Customer ID: {id}");
        }
    }
}
