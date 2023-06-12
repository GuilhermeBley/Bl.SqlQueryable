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

    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        => new List<T>(await TryGetEnumerable<T>(queryable, cancellationToken: cancellationToken));

    public static async Task<IEnumerable<T>> AsEnumerableAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        => await TryGetEnumerable<T>(queryable, cancellationToken: cancellationToken);

    public static async Task<IEnumerable<T>> ExecuteQueryableAsync<T>(this BuildersQueryable<T> queryable, CancellationToken cancellationToken = default)
        => await ((IAsyncQueryProvider)queryable.Provider).ExecuteAsync<IEnumerable<T>>(queryable.Expression, cancellationToken: cancellationToken);

    private static async Task<IEnumerable<T>> TryGetEnumerable<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        var asyncProvider = queryable.Provider as IAsyncQueryProvider;

        if (asyncProvider is null)
            return queryable.Provider.Execute<IEnumerable<T>>(queryable.Expression);

        return await asyncProvider.ExecuteAsync<IEnumerable<T>>(queryable.Expression, cancellationToken: cancellationToken);
    }
}