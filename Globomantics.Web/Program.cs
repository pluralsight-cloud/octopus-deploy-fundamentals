using Globomantics.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Bind the Azure App Setting / configuration key "DeploymentEnvironment"
// into a strongly typed options object so pages and services can inject it.
builder.Services
    .AddOptions<DeploymentOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.octopusEnvironment = configuration["DeploymentEnvironment"]
            ?? configuration["ASPNETCORE_ENVIRONMENT"]
            ?? "Unknown";
    })
    .Validate(options => !string.IsNullOrWhiteSpace(options.octopusEnvironment),
        "DeploymentEnvironment must be set.")
    .ValidateOnStart();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();
