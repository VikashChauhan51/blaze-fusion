﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BlazeFusion.Services;


/// <summary>
/// Provides a standard implementation for ICookieManager interface, allowing to store/read complex objects in cookies
/// </summary>
public sealed class CookieStorage
{
    private readonly HttpContext _httpContext;
    private readonly IPersistentState _persistentState;

    /// <summary>
    /// Returns a value type or a specific class stored in cookies
    /// </summary>
    public T Get<T>(string key, bool encryption = false, T defaultValue = default)
    {
        try
        {
            _httpContext.Request.Cookies.TryGetValue(key, out var storage);

            if (storage != null)
            {
                var json = encryption
                    ? _persistentState.Decompress(storage)
                    : storage;

                return JsonSerializer.Deserialize<T>(json);
            }
        }
        catch
        {
            //ignored
        }

        return defaultValue;
    }

    /// <summary>
    /// Default expiration time for cookies
    /// </summary>
    public static TimeSpan DefaultExpirationTime = TimeSpan.FromDays(30);


    internal CookieStorage(HttpContext httpContext, IPersistentState persistentState)
    {
        _httpContext = httpContext;
        _persistentState = persistentState;
    }

    /// <summary>
    /// Stores provided `value` in cookies
    /// </summary>
    public void Set<T>(string key, T value, bool encryption = false, TimeSpan? expiration = null)
    {
        var options = new CookieOptions
        {
            MaxAge = expiration ?? DefaultExpirationTime,
        };

        Set(key, value, encryption, options);
    }

    /// <summary>
    /// Stores in cookies a value type or a specific class with exact options to be used. Will also be encrypted if `secure` is enabled in options.
    /// </summary>
    public void Set<T>(string key, T value, bool encryption, CookieOptions options)
    {
        var response = _httpContext.Response;

        if (value != null)
        {
            var serializedValue = JsonSerializer.Serialize(value, JsonSettings.JsonSerializerSettings);
            var finalValue = encryption
                ? _persistentState.Compress(serializedValue)
                : serializedValue;

            response.Cookies.Append(key, finalValue, options);
        }
        else
        {
            response.Cookies.Delete(key);
        }
    }

    /// <summary>
    /// Deletes a cookie record
    /// </summary>
    public void Delete(string key)
    {
        var response = _httpContext.Response;
        response.Cookies.Delete(key);
    }
}
