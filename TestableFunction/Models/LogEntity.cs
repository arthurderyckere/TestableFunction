using Azure;
using Azure.Data.Tables;
using System;

namespace TestableFunction.Models;

public record LogEntity(string Message) : ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;

    public static LogEntity Create(string message)
    {
        return new LogEntity(message)
        {
            Message = message,
            PartitionKey = "Logs",
            RowKey = Guid.NewGuid().ToString(),
            Timestamp = DateTimeOffset.Now
        };
    }
}
