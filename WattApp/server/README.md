# Server

## Prerequirements

* Visual Studio 2022
* .NET 6

## Prestart

Open solution i Visual Studio 2022. When you open Visual Studio in `Package Manager Console` enter command `Update-Database`.

## Configuration

In `Server/Properties/launchSettings.json` you can change applicationUrl property.

Example:
```json
{
	"profiles": {
		"Server": {
		"commandName": "Project",
		"dotnetRunMessages": true,
		"launchBrowser": true,
		"launchUrl": "api/docs",
		"applicationUrl": "https://localhost:7146;http://localhost:5146",
		"environmentVariables": {
			"ASPNETCORE_ENVIRONMENT": "Development"
		}
		},
		"IIS Express": {
		"commandName": "IISExpress",
		"launchBrowser": true,
		"environmentVariables": {
			"ASPNETCORE_ENVIRONMENT": "Development"
		}
		}
	}
}
```

You can set other applicationUrl instead of "https://localhost:7146;http://localhost:5146". If you want to have more urls, put ;(semicolon) between two urls.

## Start server

Press `Run` button.

[Go to home](../../README.md)