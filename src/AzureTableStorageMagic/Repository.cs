using System;
using System.Net;
using System.Threading.Tasks;
using AzureTableStorageMagic.Infrastructure;
using Common.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic
{
    /// <summary>
    ///     <see cref="Repository{TEntity}" /> implements <see cref="IRepository{TEntity}"/> and provides the basic CRUD operations for an Azure table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity the table stores.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : ITableEntity
    {
        // ReSharper disable once StaticMemberInGenericType
        // The following LogNameFormat was chosen because it works best with Log2Console. 
        // Log2Console splits names at dot. {0}.{1}<TEntity> results in a branch on loggers tree ending with >.
        private static readonly string LogNameFormat = string.Format("{0}.{1}.TEntity", typeof(Repository<>).Namespace, typeof(Repository<>).Name).Replace("`1", "");

        private readonly CloudTable _cloudTable;
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly IEntityValidator _entityValidator;
        private readonly IHttpStatusCodeValidator _httpStatusCodeValidator;
        private readonly ILog _log;

        public Repository(string connectionString, string tableName)
            : this(connectionString, tableName, new EntityValidator(), new HttpStatusCodeValidator())
        {
        }

        public Repository(string connectionString, string tableName, IEntityValidator entityValidator, IHttpStatusCodeValidator httpStatusCodeValidator)
        {
            _log = LogManager.GetLogger(LogNameFormat.Replace("TEntity", typeof(TEntity).FullName));

            _connectionString = connectionString;
            _tableName = tableName;
            _entityValidator = entityValidator;
            _httpStatusCodeValidator = httpStatusCodeValidator;

            _cloudTable = Table.GetCloudTable(connectionString, tableName);
        }

        /// <summary>
        /// Adds the entity to the Azure table.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///   <see cref="AddEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        public async Task AddEntity(TEntity entity)
        {
            await ExecuteOperation(entity, TableOperation.Insert(entity), "Insert", "Adding", "Added");
        }

        /// <summary>
        /// Deletes the entity from the Azure table.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to delete.</param>
        /// <param name="rowKey">The row key of the entity to delete.</param>
        /// <returns>
        ///   <see cref="DeleteEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        public Task DeleteEntity(string primaryKey, string rowKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an entity from the Azure table.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to get.</param>
        /// <param name="rowKey">The row key of the entity to get.</param>
        /// <returns>
        ///   <see cref="GetEntity" /> is an asynchronous method and returns <see cref="Task{TEntity}" />.
        /// </returns>
        public Task<TEntity> GetEntity(string primaryKey, string rowKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an entity in the Azure table.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        ///   <see cref="UpdateEntity" /> is an asynchronous method and returns <see cref="Task" />.
        /// </returns>
        public Task UpdateEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteOperation(TEntity entity, TableOperation operation, string operationName, string doingVerb, string didVerb)
        {
            _log.TraceFormat("{0} entity. PartitionKey: '{1}', RowKey: '{2}'", doingVerb, entity.PartitionKey, entity.RowKey);

            try
            {
                _entityValidator.Validate(entity);

                var result = await _cloudTable.ExecuteAsync(operation);

                if (!_httpStatusCodeValidator.IsOK(result.HttpStatusCode))
                {
                    throw new WebException(string.Format("{0} operation failed with HttpStatusCode '{1}'. Surprised CloudTable didn't throw the error itself.", operationName, (HttpStatusCode)result.HttpStatusCode));
                }
            }
            catch (Exception exception)
            {
                throw new RepositoryException(string.Format("Cannot {0} entity.", operationName.ToLower()), _connectionString, _tableName, entity.PartitionKey, entity.RowKey, exception);
            }

            _log.TraceFormat("{0} entity. PartitionKey: '{1}', RowKey: '{2}'", didVerb, entity.PartitionKey, entity.RowKey);
        }
    }
}