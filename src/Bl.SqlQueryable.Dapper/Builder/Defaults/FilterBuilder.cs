using System.Linq.Expressions;

namespace Bl.SqlQueryable.Dapper.Builder.Defaults;

internal class FilterBuilder : ISqlBuilder
{

    public IEnumerable<KeyValuePair<string, object?>> ApplyInQueryContext(QueryContext queryContext)
    {
        var ctx = new FilterContext(queryContext.Expression);
        return Enumerable.Empty<KeyValuePair<string, object?>>();
    }

    private class FilterContext : ExpressionVisitor
    {
        public FilterContext(Expression expression)
        {
             
        }
    }
}