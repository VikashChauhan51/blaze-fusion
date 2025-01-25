using BlazeFusion.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazeFusion;

/// <summary>
/// Blaze extensions to IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures services required by 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddBlaze(this IServiceCollection services, Action<BlazeOptions> options = null)
    {
        var BlazeOptions = new BlazeOptions();
        options?.Invoke(BlazeOptions);
        services.AddSingleton(BlazeOptions);
        services.TryAddSingleton<IPersistentState, PersistentState>();

        return services;
    }
}
