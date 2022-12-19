using Azure;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestableFunction.Test.Integration.Simple.Fakes;

public class FakeTableClient : TableClient
{
    public Dictionary<string, object> Entities = new();

    public override Task<Response> AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Add(entity.RowKey, entity);

        var response = new FakeResponse();

        return Task.FromResult((Response)response);
    }
}
