using System.Linq.Expressions;

namespace Bl.SqlQueryable.Dapper.Builder;

public class QueryContext
{
    public string Query { get; set; }
    public Expression Expression { get; set; }

    public QueryContext(string query, Expression expression)
    {
        Query = query;
        Expression = expression;
    }
}