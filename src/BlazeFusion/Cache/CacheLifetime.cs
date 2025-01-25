namespace BlazeFusion;

/// <summary>
/// Defines lifetime of a cached value
/// </summary>
public enum CacheLifetime : byte
{
    /// <summary>
    /// Value will be kept in cache during one request
    /// </summary>
    Request,

    /// <summary>
    /// Value will be kept in the current application's instance cache 
    /// </summary>
    Application
}
