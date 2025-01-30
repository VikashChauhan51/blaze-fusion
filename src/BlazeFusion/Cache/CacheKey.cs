namespace BlazeFusion;

/// <summary>
/// Represents a cache key with an associated delegate.
/// </summary>
/// <param name="key">The string key for the cache entry.</param>
/// <param name="delegateKey">The delegate associated with the cache entry.</param>
internal sealed record CacheKey(string key, Delegate delegateKey);
