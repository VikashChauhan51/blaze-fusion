namespace BlazeFusion;

/// <summary>
/// Represents the different types of results that a component can return.
/// </summary>
internal enum ComponentResultType : byte
{
    /// <summary>
    /// Indicates an empty result.
    /// </summary>
    Empty,

    /// <summary>
    /// Indicates a file result.
    /// </summary>
    File,

    /// <summary>
    /// Indicates a challenge result.
    /// </summary>
    Challenge,

    /// <summary>
    /// Indicates a sign-in result.
    /// </summary>
    SignIn,

    /// <summary>
    /// Indicates a sign-out result.
    /// </summary>
    SignOut
}
