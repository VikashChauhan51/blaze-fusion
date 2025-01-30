using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace BlazeFusion;

/// <summary>  
/// Provides access to embedded script files within an assembly.  
/// </summary>  
internal sealed class ScriptsFileProvider : IFileProvider
{
    private readonly EmbeddedFileProvider _embeddedFileProvider;

    /// <summary>  
    /// Initializes a new instance of the <see cref="ScriptsFileProvider"/> class.  
    /// </summary>  
    /// <param name="assembly">The assembly containing the embedded script files.</param>  
    public ScriptsFileProvider(Assembly assembly) =>
        _embeddedFileProvider = new EmbeddedFileProvider(assembly);

    /// <summary>  
    /// Gets the directory contents for the specified subpath.  
    /// </summary>  
    /// <param name="subpath">The path that identifies the directory.</param>  
    /// <returns>The contents of the directory.</returns>  
    public IDirectoryContents GetDirectoryContents(string subpath) =>
        _embeddedFileProvider.GetDirectoryContents(subpath);

    /// <summary>  
    /// Gets the file information for the specified subpath.  
    /// </summary>  
    /// <param name="subpath">The path that identifies the file.</param>  
    /// <returns>The file information.</returns>  
    public IFileInfo GetFileInfo(string subpath) =>
        _embeddedFileProvider.GetFileInfo(subpath switch
        {
            "/blaze.js" => "/Scripts.blaze.js",
            "/blaze/blaze.js" => "/Scripts.blaze.js",
            "/blaze/alpine.js" => "/Scripts.AlpineJs.alpinejs-combined.min.js",
            _ => subpath
        });

    /// <summary>  
    /// Creates a change token for the specified filter.  
    /// </summary>  
    /// <param name="filter">The filter pattern to watch.</param>  
    /// <returns>The change token.</returns>  
    public IChangeToken Watch(string filter) =>
        _embeddedFileProvider.Watch(filter);
}
