# Server

## Prerequirements

* .NET 6+ installed

## Prestart

Open solution i Visual Studio 2022. When you open Visual Studio in `Package Manager Console` enter command `Update-Database`.
If you don't have Visual Strudio, open terminal in `Server` folder and run command:
```sh
dotnet ef database update
```



## Configuration

In `Server/appsettings.json` you should enter 

### Development configuration
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

### **Visual Studio**  
Press `Run` button.  

### **Console**
```sh
cd ./Server && dotnet run
```  

## Build
Run commamd
```sh
dotnet publish
```

### Start build

```sh
cd ./Server/bin/Release/net<FRAMEWORK>/publish &&  dotnet Server.dll --urls "<url>"
```

[Go to home](../../README.md)