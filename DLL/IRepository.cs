using DAL.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository : IDisposable
    {
        //Agregar una nueva entidad en la base de datos
        Task<TEntity> CreateAsync<TEntity>(TEntity toCreate) where TEntity: class;

        //Para eliminar una entidad
        Task<bool> DeleteAsync<TEntity>(TEntity toDelete) where TEntity: class;

        //Para Actualizar una entidad
        Task<bool> UpdateAsync<TEntity>(TEntity toUpdate) where TEntity: class;

        //Para recuperar una entidad en base en un criterio
        Task<TEntity> RetreiveAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity: class;

        //Para recuperar un conjunto de entidad en base en un criterio
        Task<List<TEntity>>FilterAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity: class; 
    }
}
