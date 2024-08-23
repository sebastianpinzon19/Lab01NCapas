using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class SupplierExceptions : Exception
    {
        public SupplierExceptions() : base("No Supplier found in the database.")
        {
        }

        private SupplierExceptions(string message) : base(message)
        {
        }

        public static void ThrowSupplierAlreadyExistsException(string companyName, string contactName)
        {
            throw new SupplierExceptions($"A Supplier with the name '{companyName}' and contact '{contactName}' already exists.");
        }

        public static void ThrowInvalidSupplierIdException(int id)
        {
            throw new SupplierExceptions($"Invalid Supplier ID: {id}");
        }
    }
}
