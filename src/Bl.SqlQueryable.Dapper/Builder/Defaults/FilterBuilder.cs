using System.Diagnostics.CodeAnalysis;
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
        private readonly IList<MethodCallExpression> _whereExpressions = new List<MethodCallExpression>();
        public IEnumerable<MethodCallExpression> WhereExpressions => _whereExpressions;

        public FilterContext(Expression expression)
        {
             
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Where")
                _whereExpressions.Add(node);

            return base.VisitMethodCall(node);
        }
    }
}