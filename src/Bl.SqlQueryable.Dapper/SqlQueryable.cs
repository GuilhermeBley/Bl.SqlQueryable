using System.Collections;
using System.Data;
using System.Linq.Expressions;
using Bl.SqlQueryable.Dapper.Builder;
using Bl.SqlQueryable.Dapper.Provider;
using Dapper;

namespace Bl.SqlQueryable.Dapper;

public class SqlQueryable<T> : IQueryable<T>
{
    private static IEnumerable<ISqlBuilder> _defaultBuilders
        = Enumerable.Empty<ISqlBuilder>();
    protected virtual IEnumerable<ISqlBuilder> Builders { get; } = _defaultBuilders;
    public Type ElementType => typeof(T);

    public Expression Expression { get; private set; }

    public IQueryProvider Provider { get; private set; }

    public SqlQueryable(
        string query, 
        System.Data.IDbConnection connection,
        DynamicParameters dynamicParameters,
        IDbTransaction? transaction = null, 
        bool buffered = true, 
        int? commandTimeout = null, 
        CommandType? commandType = null)
    {
        Provider = new SqlQueryableProvider<T>(
            query: query, 
            connection: connection, 
            builders: Builders, 
            parameters: dynamicParameters, 
            transaction: transaction, 
            buffered: buffered, 
            commandTimeout: commandTimeout,
            commandType: commandType);
        Expression = Expression.Constant(this);
    }

    public IEnumerator<T> GetEnumerator()
        => Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}