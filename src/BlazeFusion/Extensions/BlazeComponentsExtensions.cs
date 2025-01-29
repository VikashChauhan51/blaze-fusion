using BlazeFusion.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BlazeFusion;
internal static class BlazeComponentsExtensions
{

    public static void MapBlazeComponent(this IEndpointRouteBuilder app, Type componentType)
    {
        var componentName = componentType.Name;

        app.MapPost($"/blaze/{componentName}/{{method?}}", async (
            [FromServices] IServiceProvider serviceProvider,
            [FromServices] IViewComponentHelper viewComponentHelper,
            [FromServices] BlazeOptions BlazeOptions,
            [FromServices] IAntiforgery antiforgery,
            [FromServices] ILogger<BlazeComponent> logger,
            HttpContext httpContext,
            string method
        ) =>
        {
            if (BlazeOptions.AntiforgeryTokenEnabled)
            {
                try
                {
                    await antiforgery.ValidateRequestAsync(httpContext);
                }
                catch (AntiforgeryValidationException exception)
                {
                    logger.LogWarning(exception, "Antiforgery token not valid");
                    var requestToken = antiforgery.GetTokens(httpContext).RequestToken;
                    httpContext.Response.Headers.Append(BlazeConsts.ResponseHeaders.RefreshToken, requestToken);
                    return Results.BadRequest(new { token = requestToken });
                }
            }

            if (httpContext.IsBlaze())
            {
                await ExecuteRequestOperations(httpContext, method);
            }

            var htmlContent = await TagHelperRenderer.RenderTagHelper(componentType, httpContext);

            if (httpContext.Response.Headers.ContainsKey(BlazeConsts.ResponseHeaders.SkipOutput))
            {
                return BlazeEmptyResult.Instance;
            }

            var content = await GetHtml(htmlContent);
            return Results.Content(content, MediaTypeNames.Text.Html);
        });
    }

    private static async Task ExecuteRequestOperations(HttpContext context, string method)
    {
        if (!context.Request.HasFormContentType)
        {
            throw new InvalidOperationException("Blaze form doesn't contain form which is required");
        }

        var BlazeData = await context.Request.ReadFormAsync();

        var formValues = BlazeData
            .Where(f => !f.Key.StartsWith("__blaze"))
            .ToDictionary(f => f.Key, f => f.Value);

        var model = BlazeData["__blaze_model"].First();
        var type = BlazeData["__blaze_type"].First();
        var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(BlazeData["__blaze_parameters"].FirstOrDefault("{}"));
        var eventData = JsonSerializer.Deserialize<BlazeEventPayload>(BlazeData["__blaze_event"].FirstOrDefault("{}"));
        var componentIds = JsonSerializer.Deserialize<string[]>(BlazeData["__blaze_componentIds"].FirstOrDefault("[]"));
        var form = new FormCollection(formValues, BlazeData.Files);

        context.Items.Add(BlazeConsts.ContextItems.RenderedComponentIds, componentIds);
        context.Items.Add(BlazeConsts.ContextItems.BaseModel, model);
        context.Items.Add(BlazeConsts.ContextItems.Parameters, parameters);

        if (eventData != null && eventData.Name != null)
        {
            context.Items.Add(BlazeConsts.ContextItems.EventName, eventData.Name);
            context.Items.Add(BlazeConsts.ContextItems.EventData, eventData.Data);
            context.Items.Add(BlazeConsts.ContextItems.EventSubject, eventData.Subject);
        }

        if (!string.IsNullOrWhiteSpace(method) && type != "event")
        {
            context.Items.Add(BlazeConsts.ContextItems.MethodName, method);
        }

        if (form.Any() || form.Files.Any())
        {
            context.Items.Add(BlazeConsts.ContextItems.RequestForm, form);
        }
    }

    private static async Task<string> GetHtml(IHtmlContent htmlContent)
    {
        await using var writer = new StringWriter();
        htmlContent.WriteTo(writer, HtmlEncoder.Default);
        await writer.FlushAsync();
        return writer.ToString();
    }
}
