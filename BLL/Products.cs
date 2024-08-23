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
    public class Products
    {
        public async Task<Product> CreateAsync(Product product)
        {
            Product productResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre del producto existe para el mismo proveedor
                Product productSearch = await repository.RetreiveAsync<Product>(
                    p => p.ProductName == product.ProductName && p.SupplierId == product.SupplierId);

                if (productSearch == null)
                {
                    // No existe, podemos crearlo
                    productResult = await repository.CreateAsync(product);
                }
                else
                {
                    // Lanzar excepción si el producto ya existe para el mismo proveedor
                    ProductExceptions.ThrowProductAlreadyExistsException(productSearch.ProductName, productSearch.SupplierId);
                }
                return productResult;
            }
        }

        public async Task<Product> RetrieveByIDAsync(int id)
        {
            Product result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Product product = await repository.RetreiveAsync<Product>(p => p.Id == id);

                // Verificar si se encontró el producto
                if (product == null)
                {
                    // Lanzar una excepción si el producto no se encontró
                    ProductExceptions.ThrowInvalidProductIdException(id);
                }
                return product;
            }
        }

        public async Task<List<Product>> RetrieveAllAsync()
        {
            List<Product> result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Definir el criterio de filtro para obtener todos los productos.
                Expression<Func<Product, bool>> allProductsCriteria = x => true;
                result = await repository.FilterAsync<Product>(allProductsCriteria);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del producto no exista para el mismo proveedor
                Product productSearch = await repository.RetreiveAsync<Product>(
                    p => p.ProductName == product.ProductName && p.SupplierId == product.SupplierId && p.Id != product.Id);

                if (productSearch == null)
                {
                    // No existe, podemos actualizar
                    result = await repository.UpdateAsync(product);
                }
                else
                {
                    // Lanzar excepción si no se pudo modificar
                    ProductExceptions.ThrowProductAlreadyExistsException(productSearch.ProductName, productSearch.SupplierId);
                }
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;
            // Buscar un producto por su ID
            var product = await RetrieveByIDAsync(id);
            if (product != null)
            {
                // Eliminar el producto
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    result = await repository.DeleteAsync(product);
                }
            }
            else
            {
                // Lanzar excepción si el producto no existe
                ProductExceptions.ThrowInvalidProductIdException(id);
            }
            return result;
        }
    }
}
