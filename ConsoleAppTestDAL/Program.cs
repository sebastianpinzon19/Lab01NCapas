using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppTestDAL
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await CreateAsync().ConfigureAwait(false);
            //await RetreiveAsync().ConfigureAwait(false);
            //UpdateAsync().GetAwaiter().GetResult();
            //await UpdateAsync.ConfigureAwait(false);
            //await FilterAsync().ConfigureAwait(false);
            //await DeleteAsync().ConfigureAwait(false);

        }
        static async Task CreateAsync()
        {
            // Crear una instancia de un nuevo cliente
            Customer customer = new Customer
            {
                FirstName = "Vladimir",
                LastName = "Cortés",
                City = "Bogotá",
                Country = "Colombia",
                Phone = "3144427602"
            };

            using (var repository = RepositoryFactory.CreateRepository())
            {
                try
                {
                    var createdCustomer = await repository.CreateAsync(customer);
                    Console.WriteLine($"Added Customer: {createdCustomer.LastName} {createdCustomer.FirstName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        static async Task RetreiveAsync()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                try
                {
                    Expression<Func<Customer, bool>> criteria = c => c.FirstName == "Vladimir" && c.LastName == "Cortés";
                    var customer = await repository.RetreiveAsync(criteria);

                    if (customer != null)
                    {
                        Console.WriteLine($"Retriveid Customer:{customer.FirstName} \t {customer.LastName} \t City: {customer.City} \t Country: {customer.Country}");
                    }
                    else
                    {
                        Console.WriteLine("Customer not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        static async Task UpdateAsync()
        {
            //Supuesto: Existe el objeto a modificar
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var customerToUpdate = await repository.RetreiveAsync<Customer>(c => c.Id == 78);
                if (customerToUpdate != null)
                {
                    customerToUpdate.FirstName = "Liu";
                    customerToUpdate.LastName = "Wong";
                    customerToUpdate.City = "Toronto";
                    customerToUpdate.Country = "Canada";
                    customerToUpdate.Phone = "+14337 635039";
                }
                try
                {
                    bool updated = await repository.UpdateAsync(customerToUpdate);
                    if (updated)
                    {
                        Console.WriteLine("Customer updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Customer updated failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }
        }
        static async Task FilterAsync()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Expression<Func<Customer, bool>> criteria = c => c.Country == "USA";
                var Customers = await repository.FilterAsync(criteria);
                foreach (var customer in Customers)
                {
                    Console.WriteLine($"Customer {customer.FirstName}    {customer.LastName} \t from:{customer.City}");
                }
            }
        }
        static async Task DeleteAsync()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Expression<Func<Customer, bool>> criteria = customer => customer.Id == 94;
                var customerToDelete = await repository.RetreiveAsync(criteria);
                if (customerToDelete != null)
                {
                    bool deleted = await repository.DeleteAsync(customerToDelete);
                    Console.WriteLine(deleted ? "Customer successfull. " : "Failed Customer");
                }
            }
        }
    }
}

