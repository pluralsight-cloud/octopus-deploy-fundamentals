# Globomantics Web

Simple ASP.NET Core 8 Razor Pages site for Globomantics. The home page and header
badge display the value of the configuration key **`DeploymentEnvironment`**.

## Run locally

```bash
dotnet run --project Globomantics.Web.csproj
```

Open http://localhost:5080. You should see `Development` (from
`appsettings.Development.json` and `Properties/launchSettings.json`).

Override without changing files:

```bash
DeploymentEnvironment=Staging dotnet run --project Globomantics.Web.csproj
```

## Configuration

| Source | How it is set |
| --- | --- |
| `appsettings.json` | `"DeploymentEnvironment": "Local"` |
| `appsettings.Development.json` | `"Development"` |
| `appsettings.Production.json` | `"Production"` |
| Environment variable | `DeploymentEnvironment=Staging` |
| Azure App Service | Application Setting named `DeploymentEnvironment` |

ASP.NET Core configuration already maps environment variables and Azure
Application Settings onto `IConfiguration`. The app binds that key through
`DeploymentOptions` in `Program.cs` and injects it with `IOptions<DeploymentOptions>`.

Health check: `GET /health`.

## Deploy to Azure App Service

```bash
az group create --name rg-globomantics --location eastus

az appservice plan create \
  --name plan-globomantics \
  --resource-group rg-globomantics \
  --sku B1 \
  --is-linux

az webapp create \
  --name globomantics-web-<unique> \
  --resource-group rg-globomantics \
  --plan plan-globomantics \
  --runtime "DOTNETCORE:8.0"

az webapp config appsettings set \
  --name globomantics-web-<unique> \
  --resource-group rg-globomantics \
  --settings DeploymentEnvironment=Production ASPNETCORE_ENVIRONMENT=Production

cd Globomantics.Web
dotnet publish -c Release -o ./publish
cd publish
zip -r ../deploy.zip .
az webapp deploy \
  --name globomantics-web-<unique> \
  --resource-group rg-globomantics \
  --src-path ../deploy.zip \
  --type zip
```

Swap slots by giving each slot its own `DeploymentEnvironment` value
(`Staging`, `Production`, and so on). The UI will show whichever setting that
slot has.
