using Microsoft.AspNetCore.DataProtection;
using System.IO.Compression;
using System.Text;

namespace BlazeFusion;

/// <summary>
/// Provides methods to compress and decompress string values, with optional data protection.
/// </summary>
internal sealed class PersistentState : IPersistentState
{
    private readonly IDataProtector _protector;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistentState"/> class.
    /// </summary>
    /// <param name="provider">The data protection provider.</param>
    public PersistentState(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector(nameof(PersistentState));
    }

    /// <summary>
    /// Compresses the specified string value using Brotli compression.
    /// </summary>
    /// <param name="value">The string value to compress.</param>
    /// <returns>The compressed string, encoded in Base64.</returns>
    public string Compress(string value)
    {
        var inputBytes = Encoding.UTF8.GetBytes(value);
        using var outputStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress))
        {
            brotliStream.Write(inputBytes, 0, inputBytes.Length);
        }

        return Convert.ToBase64String(outputStream.ToArray());
    }

    /// <summary>
    /// Decompresses the specified string value using Brotli decompression.
    /// If decompression fails, attempts to unprotect the value using data protection.
    /// </summary>
    /// <param name="value">The string value to decompress.</param>
    /// <returns>The decompressed string.</returns>
    public string Decompress(string value)
    {
        try
        {
            using var memoryStream = new MemoryStream(Convert.FromBase64String(value));
            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
            {
                brotliStream.CopyTo(outputStream);
            }

            return Encoding.UTF8.GetString(outputStream.ToArray());
        }
        catch (FormatException)
        {
            return _protector.Unprotect(value);
        }
    }
}
