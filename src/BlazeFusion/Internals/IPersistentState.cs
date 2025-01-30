namespace BlazeFusion;

/// <summary>
/// Provides methods to compress and decompress string values.
/// </summary>
internal interface IPersistentState
{
    /// <summary>
    /// Compresses the specified string value.
    /// </summary>
    /// <param name="value">The string value to compress.</param>
    /// <returns>The compressed string.</returns>
    string Compress(string value);

    /// <summary>
    /// Decompresses the specified string value.
    /// </summary>
    /// <param name="value">The string value to decompress.</param>
    /// <returns>The decompressed string.</returns>
    string Decompress(string value);
}
