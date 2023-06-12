using System.Data;
using Bl.SqlQueryable.Dapper;
using Bl.SqlQueryable.Dapper.Tests.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Bl.SqlQueryable.Dapper.Tests.Tests;

public class QueryableTest : DefaultHost
{
    [Fact]
    public async Task Queryable_GetList_Success()
    {
        IDbConnection c = CreateScope().GetRequiredService<IDbConnection>();
        var result = await c.Queryable<Queryable4Columns>("SELECT * FROM queryable.queryable4columns;").ToListAsync();
    }
}