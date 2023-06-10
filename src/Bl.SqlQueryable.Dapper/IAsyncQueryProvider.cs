using System.Linq.Expressions;

namespace Bl.SqlQueryable.Dapper;

public interface IAsyncQueryProvider : IQueryProvider
{
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
} 