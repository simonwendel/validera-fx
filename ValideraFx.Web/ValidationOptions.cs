// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Web;

/// <summary>
/// Provides configuration options for controlling validation behavior.
/// </summary>
public class ValidationOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the model state must be valid before proceeding with processing.
    /// </summary>
    /// <remarks>
    /// This is useful in ASP.NET MVC applications where you want to ensure that the model state is valid
    /// before executing an action method. If the model state is invalid, the action method will not be executed,
    /// and an error response will be returned instead. Explicit checking <c>ModelState.IsValid</c> is not required.
    /// </remarks>
    public bool EnforceValidModelState { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only explicitly validated types should be allowed for use in action
    /// methods. Setting this to <c>true</c> will scan all action method parameters on startup and throw an exception
    /// if any parameter is not explicitly validated.
    /// </summary>
    public bool EnforceValidatedTypes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all potentially unsafe values should be excluded from error messages.
    /// Setting this to <c>true</c> will prevent the rendering of values in error messages.
    /// </summary>
    public bool DontRenderValues { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether strict validation mode is enabled.
    /// When enabled, all enforcement options are turned on:
    /// <list>
    ///     <item>
    ///         <description><see cref="EnforceValidModelState"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="EnforceValidatedTypes"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="DontRenderValues"/></description>
    ///     </item>
    /// </list>
    /// </summary>
    public bool StrictValidationMode
    {
        get => EnforceValidModelState && EnforceValidatedTypes && DontRenderValues;
        set
        {
            if (!value)
            {
                return;
            }

            EnforceValidModelState = true;
            EnforceValidatedTypes = true;
            DontRenderValues = true;
        }
    }
}