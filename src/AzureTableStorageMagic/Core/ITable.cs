using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic.Core
{
    /// <summary>
    ///     <see cref="ITable{TEntity}" /> provide the basic CRUD operations for an Azure table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity the table stores.</typeparam>
    public interface ITable<TEntity> where TEntity : ITableEntity
    {
        /// <summary>
        ///     Adds the entity to the Azure table.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///     <see cref="AddEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        Task AddEntity(TEntity entity);

        /// <summary>
        ///     Deletes the entity from the Azure table.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to delete.</param>
        /// <param name="rowKey">The row key of the entity to delete.</param>
        /// <returns>
        ///     <see cref="DeleteEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        Task DeleteEntity(string primaryKey, string rowKey);

        /// <summary>
        ///     Gets an entity from the Azure table.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to get.</param>
        /// <param name="rowKey">The row key of the entity to get.</param>
        /// <returns>
        ///     <see cref="GetEntity" /> is an asynchronous method and returns <see cref="Task{TEntity}" />.
        /// </returns>
        Task<TEntity> GetEntity(string primaryKey, string rowKey);

        /// <summary>
        ///     Updates an entity in the Azure table.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        ///     <see cref="UpdateEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        Task UpdateEntity(TEntity entity);
    }
}