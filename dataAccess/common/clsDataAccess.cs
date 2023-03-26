using System.Data;
using System.Data.SqlClient;
using chessAPI.dataAccess.interfaces;
using Dapper;

namespace chessAPI.dataAccess.common;

public abstract class clsDataAccess<TEntity, TKey, TC>
        where TEntity : relationalEntity<TKey, TC>, new()
        where TKey : IEquatable<TKey>
        where TC : struct
    {
        internal IRelationalContext<TC> rkm;
        internal IEnumerable<TEntity>? cache;
        protected virtual ISQLData queries { get; private set; }
        internal DynamicParameters? DBParams;
        internal readonly ILogger<clsDataAccess<TEntity, TKey, TC>> logger;

        protected clsDataAccess(IRelationalContext<TC> rkm,
                                ISQLData queries,
                                ILogger<clsDataAccess<TEntity, TKey, TC>> logger)
        {
            this.rkm = rkm;
            this.queries = queries;
            this.logger = logger;
            this.cache = null;
        }

        protected bool cacheddata
        {
            get
            {
                return (cache != null) && (cache.Any() == true);
            }
        }

        protected abstract DynamicParameters keyAsParams(TKey key);
        protected abstract DynamicParameters fieldsAsParams(TEntity entity); //Populate DBParams with all fields in the table

        protected async Task<IEnumerable<T>?> get<T>(DynamicParameters param, string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                return await rkm.trn.Connection.QueryAsync<T>(query, param, rkm.trn, null, commandType).ConfigureAwait(false);
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-GET<T> + {" + query + "} Error: " + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Timeout al hacer SQL-GET<T> + {" + query + "} Error: " + ex.ToString());
                throw;
            }
        }

        protected async Task<IEnumerable<TEntity>?> get(DynamicParameters param, string query)
        {
            return await get<TEntity>(param, query).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<TEntity>?> getALL(DynamicParameters param)
        {
            return await get<TEntity>(param, queries.SQLGetAll).ConfigureAwait(false);
        }

        protected async Task<TEntity?> getEntity(TKey key, bool UseCache = true)
        {
            TEntity? result;
            if (UseCache == true && cache != null)
            {
                result = cache.Where(x => x.key.Equals(key)).FirstOrDefault();
                if (result != null) return result;
            }
            var p = keyAsParams(key);
            var data = await get(p, queries.SQLDataEntity).ConfigureAwait(false);
            cache = data;

            return data?.FirstOrDefault();
        }

        protected async Task<TResult> add<TResult>(DynamicParameters param)
        {
            try
            {
                IEnumerable<TResult> result = await rkm.trn.Connection.QueryAsync<TResult>(sql: queries.NewDataEntity,
                    param: param,
                    transaction: rkm.trn,
                    commandType: CommandType.Text).ConfigureAwait(false);
                return result.First();
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-ADD<TResult> {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Error al hacer SQL-ADD<TResult> (TimeOut) {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Error general al hacer SQL-ADD<TResult> {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
        }

        protected async Task<bool> add(DynamicParameters param)
        {
            try
            {
                var result = await rkm.trn.Connection.QueryAsync<TEntity>(sql: queries.NewDataEntity,
                                                         param: param,
                                                         transaction: rkm.trn,
                                                         commandType: CommandType.Text).ConfigureAwait(false);
                if (result != null && result.Any() == true)
                {
                    if (cache == null)
                    {
                        cache = result;
                    }
                    else
                    {
                        cache.ToList().AddRange(result);
                    }
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-ADD<bool> {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Error al hacer SQL-ADD<bool> (TimeOut) {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Error general al hacer SQL-ADD<bool> {" + queries.NewDataEntity + "} Error:" + ex.ToString());
                throw;
            }
        }

        protected async Task add(TEntity item)
        {
            try
            {
                DBParams = fieldsAsParams(item);
                var result = await rkm.trn.Connection.ExecuteAsync(queries.NewDataEntity, DBParams, rkm.trn, null, CommandType.Text).ConfigureAwait(false);
                if (result != 1)
                {
                    throw new DBConcurrencyException("Error de concurrencia al hacer INSERT en query " + queries.NewDataEntity);
                }
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-ADD<bool> + {" + queries.NewDataEntity + "} Error: " + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Timeout al hacer SQL-ADD<bool> + {" + queries.NewDataEntity + "} Error: " + ex.ToString());
                throw;
            }
        }

        protected async Task<bool> delete(DynamicParameters param)
        {
            try
            {
                var result = await rkm.trn.Connection.ExecuteAsync(sql: queries.DeleteDataEntity,
                                                                   param: param,
                                                                   transaction: rkm.trn,
                                                                   commandType: CommandType.Text).ConfigureAwait(false);
                return result == 1;
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-DELETE {" + queries.DeleteDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Error al hacer SQL-DELETE (TimeOut) {" + queries.DeleteDataEntity + "} Error:" + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Error general al hacer SQL-DELETE {" + queries.DeleteDataEntity + "} Error:" + ex.ToString());
                throw;
            }
        }

        protected async Task delete(TKey key, TC? RowVersion)
        {
            var p = keyAsParams(key);
            await set(p, RowVersion, queries.DeleteDataEntity).ConfigureAwait(false);
        }

        protected async Task setAll(TEntity item, TC? RowVersion)
        {
            var p = fieldsAsParams(item);
            await set(p, RowVersion, queries.UpdateWholeEntity).ConfigureAwait(false);
        }

        protected async Task<TResult?> set<TResult>(DynamicParameters param, TC? rowVersion, string query, Action<TResult> setFields)
        {
            try
            {
                if (rowVersion.HasValue)
                {
                    rkm.concurrencyHandler.optimisticConcurrencyColumnAsParam(rowVersion.Value, ref param);
                }
                var x = await rkm.trn.Connection.QueryAsync<TResult>(sql: query, param: param,
                                                                     transaction: rkm.trn,
                                                                     commandType: CommandType.Text).ConfigureAwait(false);
                if (x != null && x.Any())
                {
                    if (cacheddata == true)
                    {
                        setFields?.Invoke(x.First());
                    }
                    return x.First();
                }
                throw new DBConcurrencyException("Error de concurrencia en la base de datos");
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-SET {" + query + "} Error:" + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Error al hacer SQL-SET (TimeOut) {" + query + "} Error:" + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Error general al hacer SQL-SET {" + query + "} Error:" + ex.ToString());
                throw;
            }
        }

        protected async Task<bool> set(DynamicParameters param, TC? rowVersion, string query, Action? setFields)
        {
            try
            {
                if (rowVersion.HasValue)
                {
                    rkm.concurrencyHandler.optimisticConcurrencyColumnAsParam(rowVersion.Value, ref param);
                }
                var x = await rkm.trn.Connection.ExecuteAsync(sql: query,
                                                              param: param,
                                                              transaction: rkm.trn,
                                                              commandType: CommandType.Text).ConfigureAwait(false);
                if (x > 0)
                {
                    if (cacheddata == true)
                    {
                        setFields?.Invoke();
                    }
                }
                else
                {
                    throw new DBConcurrencyException("Error de concurrencia en la base de datos");
                }
                return true;
            }
            catch (SqlException ex)
            {
                logger.LogError("Error al hacer SQL-SET {" + query + "} Error:" + ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError("Error al hacer SQL-SET (TimeOut) {" + query + "} Error:" + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Error general al hacer SQL-SET {" + query + "} Error:" + ex.ToString());
                throw;
            }
        }

        protected async Task set(DynamicParameters param, TC? rowVersion, string query)
        {
            await set(param, rowVersion, query, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Update whole entity
        /// </summary>
        /// <param name="param"></param>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        protected async Task set(DynamicParameters param, TC? rowVersion)
        {
            await set(param, rowVersion, queries.UpdateWholeEntity, null).ConfigureAwait(false);
        }
    }