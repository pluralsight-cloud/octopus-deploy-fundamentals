namespace Globomantics.Web.Configuration;

/// <summary>
/// Strongly typed wrapper around the <c>DeploymentEnvironment</c>
/// configuration key. In Azure App Service this is typically supplied
/// as an Application Setting, which ASP.NET Core maps onto
/// <see cref="IConfiguration"/> automatically.
/// </summary>
public sealed class DeploymentOptions
{
    /// <summary>
    /// The configuration key expected in appsettings, environment
    /// variables, and Azure Application Settings.
    /// </summary>
    public const string ConfigurationKey = "DeploymentEnvironment";

    /// <summary>
    /// Resolved environment label shown in the UI
    /// (for example Development, Staging, or Production).
    /// </summary>
    public string octopusEnvironment { get; set; } = "Unknown";
}
