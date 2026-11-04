using Globomantics.Web.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Globomantics.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IOptions<DeploymentOptions> deploymentOptions,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment,
        ILogger<IndexModel> logger)
    {
        DeploymentEnvironment = deploymentOptions.Value.octopusEnvironment;
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    /// <summary>
    /// Value of the <c>DeploymentEnvironment</c> configuration key.
    /// Sourced from appsettings, environment variables, or Azure App Settings.
    /// </summary>
    public string DeploymentEnvironment { get; }

    public string HostingEnvironmentName => _hostEnvironment.EnvironmentName;

    public string ConfigurationSourceHint { get; private set; } = string.Empty;

    public void OnGet()
    {
        var raw = _configuration[DeploymentOptions.ConfigurationKey];
        ConfigurationSourceHint = raw is null
            ? "Not set in configuration; fell back to ASPNETCORE_ENVIRONMENT or Unknown."
            : "Resolved from IConfiguration[\"DeploymentEnvironment\"] (appsettings, env var, or Azure Application Setting).";

        _logger.LogInformation(
            "Home page rendered. DeploymentEnvironment={DeploymentEnvironment}, HostingEnvironment={HostingEnvironment}",
            DeploymentEnvironment,
            HostingEnvironmentName);
    }
}
