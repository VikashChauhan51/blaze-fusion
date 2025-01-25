using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace BlazeFusion;

internal class ScriptsFileProvider : IFileProvider
{
    private readonly EmbeddedFileProvider _embeddedFileProvider;

    public ScriptsFileProvider(Assembly assembly) =>
        _embeddedFileProvider = new EmbeddedFileProvider(assembly);

    public IDirectoryContents GetDirectoryContents(string subpath) =>
        _embeddedFileProvider.GetDirectoryContents(subpath);

    public IFileInfo GetFileInfo(string subpath) =>
        _embeddedFileProvider.GetFileInfo(subpath switch
        {
            "/blaze.js" => "/Scripts.blaze.js",
            "/blaze/blaze.js" => "/Scripts.blaze.js",
            "/blaze/alpine.js" => "/Scripts.AlpineJs.alpinejs-combined.min.js",
            _ => subpath
        });

    public IChangeToken Watch(string filter) =>
        _embeddedFileProvider.Watch(filter);
}
