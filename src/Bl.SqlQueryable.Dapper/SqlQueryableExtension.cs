using System.Data;
using Dapper;

namespace Bl.SqlQueryable.Dapper;

public static class SqlQueryableExtension
{
    public static IQueryable<T> Queryable<T>(this IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var dynamicParameters = new DynamicParameters(param);

        var queryable = new BuildersQueryable<T>(
            query: sql,
            connection: cnn,
            dynamicParameters: dynamicParameters,
            transaction: transaction,
            buffered: true,
            commandTimeout: commandTimeout,
            commandType: commandType);

        return queryable;
    }

    public static async Task<IEnumerable<T>> ExecuteQueryableAsync<T>(BuildersQueryable<T> queryable, CancellationToken cancellationToken = default)
        => await ((IAsyncQueryProvider)queryable.Provider).ExecuteAsync<IEnumerable<T>>(queryable.Expression, cancellationToken: cancellationToken);
}