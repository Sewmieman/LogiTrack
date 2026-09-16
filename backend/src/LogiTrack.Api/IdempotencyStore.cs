using System.Collections.Concurrent;

namespace LogiTrack.Api;

public sealed record IdempotencyResult(int StatusCode, object Body, string? Location = null);

public interface IIdempotencyStore
{
    bool TryGet(string scope, string key, out IdempotencyResult result);
    bool TryAdd(string scope, string key, IdempotencyResult result);
}

public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<string, IdempotencyResult> _items = new();
    public bool TryGet(string scope, string key, out IdempotencyResult result) => _items.TryGetValue($"{scope}:{key}", out result!);
    public bool TryAdd(string scope, string key, IdempotencyResult result) => _items.TryAdd($"{scope}:{key}", result);
}
