using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class ProductExceptions : Exception
    {
        public ProductExceptions() : base("No product found in the database.")
        {
        }

        private ProductExceptions(string message) : base(message)
        {
        }

        public static void ThrowProductAlreadyExistsException(string productName, int? supplierId)
        {
            throw new ProductExceptions($"A product with the name '{productName} and supplier id {supplierId} already exists.");
        }

        public static void ThrowInvalidProductIdException(int id)
        {
            throw new ProductExceptions($"Invalid Product ID: {id}");
        }
    }
}

