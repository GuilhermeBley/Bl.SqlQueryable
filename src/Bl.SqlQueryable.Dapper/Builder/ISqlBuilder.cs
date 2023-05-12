namespace Bl.SqlQueryable.Dapper.Builder;

public interface ISqlBuilder
{
    IEnumerable<KeyValuePair<string, object?>> ApplyInQueryContext(QueryContext queryContext);
}