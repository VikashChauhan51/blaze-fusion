using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace BlazeFusion;

public class BlazeComponent : TagHelper, IViewContextAware
{

    private string _componentId;
    private bool _skipOutput;
    private dynamic _viewBag;


    internal static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        Converters = new JsonConverter[] { new Converters.Int32Converter() }.ToList(),
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };


    /// <summary>
    /// Provides indication if ModelState is valid
    /// </summary>
    protected bool IsValid { get; private set; }

    /// <summary>
    /// Provides component's key value
    /// </summary>
    [JsonProperty]
    public string Key { get; set; }

    /// <summary>
    /// Default identifier used to specify place of the page to replace when during location change
    /// </summary>
    public const string LocationTargetId = "blaze";

    /// <summary>
    /// Provides list of already accessed component's properties  
    /// </summary>
    [HtmlAttributeNotBound]
    public HashSet<string> TouchedProperties { get; set; } = new();

    /// <summary>
    /// Determines if the whole model was accessed already
    /// </summary>
    [HtmlAttributeNotBound]
    public bool IsModelTouched { get; set; }


    /// <summary>
    /// Unique component identifier
    /// </summary>
    [HtmlAttributeNotBound]
    public string ComponentId => _componentId;

    /// <summary>
    /// View context
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// View data
    /// </summary>
    [HtmlAttributeNotBound]
    public ViewDataDictionary ViewData => ViewContext.ViewData;

    /// <summary>
    /// Model state
    /// </summary>
    [HtmlAttributeNotBound]
    public ModelStateDictionary ModelState => ViewContext.ModelState;

    /// <summary>
    /// HttpContext
    /// </summary>
    [HtmlAttributeNotBound]
    public HttpContext HttpContext => ViewContext.HttpContext;

    /// <summary>
    /// Request
    /// </summary>
    [HtmlAttributeNotBound]
    public HttpRequest Request => ViewContext.HttpContext.Request;

    /// <summary>
    /// Request
    /// </summary>
    [HtmlAttributeNotBound]
    public HttpResponse Response => ViewContext.HttpContext.Response;

    /// <summary>
    /// RouteData
    /// </summary>
    [HtmlAttributeNotBound]
    public RouteData RouteData => ViewContext.RouteData;

    /// <summary>
    /// Url helper
    /// </summary>
    [HtmlAttributeNotBound]
    public IUrlHelper Url { get; set; }

    /// <summary>
    /// Properties
    /// </summary>
    public object Params { get; set; }

    public void Contextualize(ViewContext viewContext)
    {
        ViewContext = viewContext;
    }
}
