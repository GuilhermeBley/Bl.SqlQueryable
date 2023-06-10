using System.Data;
using System.Linq.Expressions;
using Bl.SqlQueryable.Dapper.Builder;
using Dapper;

namespace Bl.SqlQueryable.Dapper.Provider;

internal class SqlQueryableProvider<T> : IAsyncQueryProvider
{
    private readonly string _query;
    private readonly IDbConnection _connection;
    private readonly IEnumerable<ISqlBuilder> _builders;
    private readonly DynamicParameters _parameters;
    private readonly IDbTransaction? _transaction;
    private readonly bool _buffered;
    private readonly int? _commandTimeout;
    private readonly CommandType? _commandType;

    public SqlQueryableProvider(
        string query, 
        System.Data.IDbConnection connection,
        IEnumerable<ISqlBuilder> builders,
        DynamicParameters parameters,
        IDbTransaction? transaction, 
        bool buffered, 
        int? commandTimeout, 
        CommandType? commandType)
    {
        _query = query;
        _connection = connection;
        _builders = builders;
        _parameters = parameters;
        _transaction = transaction;
        _buffered = buffered;
        _commandTimeout = commandTimeout;
        _commandType = commandType;
    }

    public IQueryable CreateQuery(Expression expression)
        => CreateQuery<T>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new BuildersQueryable<TElement>(
            query: _query, 
            connection: _connection,
            dynamicParameters: _parameters,
            transaction: _transaction,
            buffered: _buffered,
            commandTimeout: _commandTimeout,
            commandType: _commandType);

    public object? Execute(Expression expression)
        => Execute<T>(expression);

    public TResult Execute<TResult>(Expression expression)
    {
        var queryCtx = new QueryContext(_query, expression);

        foreach (var builder in _builders)
        {
            var generatedParameters = 
                builder.ApplyInQueryContext(queryCtx);

            AddRangeDynamicParameters(_parameters, generatedParameters);
        }

        var items = Query(queryCtx.Query, _parameters);

        return (TResult)items.AsQueryable().Provider.CreateQuery(expression);
    }

    public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var queryCtx = new QueryContext(_query, expression);

        foreach (var builder in _builders)
        {
            var generatedParameters = 
                builder.ApplyInQueryContext(queryCtx);

            AddRangeDynamicParameters(_parameters, generatedParameters);
        }

        var items = await QueryAsync(queryCtx.Query, _parameters);

        return (TResult)items.AsQueryable().Provider.CreateQuery(expression);
    }

    public IEnumerable<T> Query(string query, DynamicParameters dynamicParameters)
        => _connection.Query<T>(
            sql: query, 
            param: dynamicParameters,
            transaction: _transaction,
            buffered: _buffered,
            commandTimeout: _commandTimeout,
            commandType: _commandType);

    public async Task<IEnumerable<T>> QueryAsync(string query, DynamicParameters dynamicParameters)
        => await _connection.QueryAsync<T>(
            sql: query, 
            param: dynamicParameters,
            transaction: _transaction,
            commandTimeout: _commandTimeout,
            commandType: _commandType);

    private static void AddRangeDynamicParameters(DynamicParameters dynamicParameters, IEnumerable<KeyValuePair<string, object?>> newValuesToTryAdd)
    {
        foreach (var newValueToTryAdd in newValuesToTryAdd)
            dynamicParameters.Add(newValueToTryAdd.Key, newValueToTryAdd.Value);
    }
}