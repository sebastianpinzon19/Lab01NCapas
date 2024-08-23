using BLL.Exceptions;
using DAL;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class Customers
    {
        public async Task<Customer> CreateAsync(Customer customer)
        {
            Customer customerResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                //Buscar si el nombre del cliente existe
                Customer customerSearch = await repository.RetreiveAsync<Customer>(c => c.FirstName == customer.FirstName);
                if (customerSearch == null) 
                {
                    //No esxite podemos crearlo
                    customerResult = await repository.CreateAsync(customer);
                }
                else
                {
                    //Podriamos lanzar una Excepcion 
                    //Para notificar que el cliente ya existe.
                    //Podriamos Crear incluso una cap de exepciones
                    //Perzonalizada y consumirlas desde otras capas
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customerSearch.LastName);
                }
                return customerResult!;
            }
        }

        public async Task<Customer> RetrieveByIDAsync(int id)
        {
            Customer result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Customer customer = await repository.RetreiveAsync<Customer>(c => c.Id == id);

                // Check if customer was found
                if (customer == null)
                {
                    // Throw a CustomerNotFoundException (assuming you have this class)
                    CustomerExceptions.ThrowInvalidCustomerIdException(id);
                }
                return customer!;
            }
        }
        public async Task<List<Customer>> RetreiveAllAsync()
        {
            List<Customer> Result = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Define el criterio de filtro para obtener todos los clientes.
                Expression<Func<Customer, bool>> allCustomersCriteria = x => true;
                Result = await r.FilterAsync<Customer>(allCustomersCriteria);
            }

            return Result;
        }
        public async Task<bool> UpdateAsync(Customer customer)
        {
            bool Result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del cliente no exista
                Customer customerSearch = await repository.RetreiveAsync<Customer>
                    (c => c.FirstName == customer.FirstName && c.Id != customer.Id);
                if (customerSearch == null)
                {
                    // No existe
                    Result = await repository.UpdateAsync(customer);
                }
                else
                {
                    // Podemos implementar alguna lógica para
                    // indicar que no se pudo modificar
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return Result;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            bool Result = false;
            // Buscar un cliente para ver si tiene Orders (Ordenes de Compra)
            var customer = await RetrieveByIDAsync(id);
            if (customer != null)
            {
                // Eliminar el cliente
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    Result = await repository.DeleteAsync(customer);
                }
            }
            else
            {
                // Podemos implementar alguna lógica
                // para indicar que el producto no existe
                CustomerExceptions.ThrowInvalidCustomerIdException(id);
            }
            return Result;
        }
    }
}
