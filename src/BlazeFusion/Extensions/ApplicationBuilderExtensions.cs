﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace BlazeFusion;


/// <summary>
/// Blaze extensions to IApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds configuration for Blaze
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
    /// <param name="environment">Current environment</param>
    public static IApplicationBuilder UseBlaze(this IApplicationBuilder builder, IWebHostEnvironment environment) =>
        builder.UseBlaze(environment, Assembly.GetCallingAssembly());

    /// <summary>
    /// Adds configuration for Blaze
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
    /// <param name="environment">Current environment</param>
    /// <param name="assembly">Assembly to scan for the Blaze components</param>
    /// <returns></returns>
    public static IApplicationBuilder UseBlaze(this IApplicationBuilder builder, IWebHostEnvironment environment, Assembly assembly)
    {
        builder.UseEndpoints(endpoints =>
        {
            var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(BlazeComponent))).ToList();

            foreach (var type in types)
            {
                endpoints.MapBlazeComponent(type);
            }
        });

        var existingProvider = environment.WebRootFileProvider;

        var scriptsFileProvider = new ScriptsFileProvider(typeof(ApplicationBuilderExtensions).Assembly);
        var compositeProvider = new CompositeFileProvider(existingProvider, scriptsFileProvider);
        environment.WebRootFileProvider = compositeProvider;

        return builder;
    }

}

