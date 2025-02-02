﻿namespace BlazeFusion;

/// <summary>
/// Used in Blaze action calls for passing JavaScript expressions as parameters
/// </summary>
public static class Param
{
    /// <summary>
    /// Pass JavaScript expression as a parameter to Blaze action
    /// </summary>
    /// <param name="value">JavaScript expression</param>
    public static T JS<T>(string value) => default;

    /// <summary>
    /// Pass JavaScript expression as a parameter to Blaze action
    /// </summary>
    /// <param name="value">JavaScript expression</param>
    public static string JS(string value) => default;
}
