using BLL.Exceptions;
using DAL;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL
{
    public class Suppliers
    {
        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            Supplier supplierResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre de la empresa ya existe
                Supplier supplierSearch = await repository.RetreiveAsync<Supplier>(
                    s => s.CompanyName == supplier.CompanyName && s.Country == supplier.Country);

                if (supplierSearch == null)
                {
                    // No existe, podemos crearlo
                    supplierResult = await repository.CreateAsync(supplier);
                }
                else
                {
                    // Lanzar excepción si el proveedor ya existe
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.CompanyName , supplierSearch.ContactName);
                }
                return supplierResult;
            }
        }

        public async Task<Supplier> RetrieveByIDAsync(int id)
        {
            Supplier result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Supplier supplier = await repository.RetreiveAsync<Supplier>(s => s.Id == id);

                // Verificar si se encontró el proveedor
                if (supplier == null)
                {
                    // Lanzar una excepción si el proveedor no se encontró
                    SupplierExceptions.ThrowInvalidSupplierIdException(id);
                }
                return supplier;
            }
        }

        public async Task<List<Supplier>> RetrieveAllAsync()
        {
            List<Supplier> result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Definir el criterio de filtro para obtener todos los proveedores.
                Expression<Func<Supplier, bool>> allSuppliersCriteria = x => true;
                result = await repository.FilterAsync<Supplier>(allSuppliersCriteria);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre de la empresa no exista ya para otro proveedor en el mismo país
                Supplier supplierSearch = await repository.RetreiveAsync<Supplier>(
                    s => s.CompanyName == supplier.CompanyName && s.Country == supplier.Country && s.Id != supplier.Id);

                if (supplierSearch == null)
                {
                    // No existe, podemos actualizar
                    result = await repository.UpdateAsync(supplier);
                }
                else
                {
                    // Lanzar excepción si no se pudo modificar
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.CompanyName, supplierSearch.ContactName);
                }
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;
            // Buscar un proveedor por su ID
            var supplier = await RetrieveByIDAsync(id);
            if (supplier != null)
            {
                // Eliminar el proveedor
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    result = await repository.DeleteAsync(supplier);
                }
            }
            else
            {
                // Lanzar excepción si el proveedor no existe
                SupplierExceptions.ThrowInvalidSupplierIdException(id);
            }
            return result;
        }
    }
}
