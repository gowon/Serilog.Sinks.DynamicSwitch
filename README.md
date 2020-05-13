# Serilog.Sinks.DynamicSwitch

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Serilog.Sinks.DynamicSwitch?color=blue)](https://www.nuget.org/packages/Serilog.Sinks.DynamicSwitch)
[![Nuget](https://img.shields.io/nuget/dt/Serilog.Sinks.DynamicSwitch?color=blue)](https://www.nuget.org/packages/Serilog.Sinks.DynamicSwitch)
![build](https://github.com/gowon/Serilog.Sinks.DynamicSwitch/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/gowon/Serilog.Sinks.DynamicSwitch/branch/master/graph/badge.svg)](https://codecov.io/gh/gowon/Serilog.Sinks.DynamicSwitch)

A Serilog sink that exposes a simple interface to retrieve and change LoggingLevelSwitches at runtime.

## Installing via NuGet

To get started install the *Serilog.Sinks.DynamicSwitch* package:

```powershell
PM> Install-Package Serilog.Sinks.DynamicSwitch
```

or

```bash
dotnet add package Serilog.Sinks.DynamicSwitch
```

## Using

### Setup sinks

A DynamicSwitch sink can be set up in the standard way while constructing a `LoggerConfiguration`. A specific `LoggingLevelSwitchCollection` object can be passed to the configuration; if one is not specified, a static global collection is used that be referenced using `LoggingLevelSwitchCollection.Current`. Each DynamicSwitch must be uniquely named. The following is a simple example:

```csharp
var logLevel = new LoggingLevelSwitch(LogEventLevel.Warning);

var log = new LoggerConfiguration()
    .WriteTo.Console(levelSwitch: logLevel)
    .WriteTo.DynamicSwitch("Test", logLevel)
    .CreateLogger();
```

If using `appsettings.json` for configuration, the following example shows how to configure DynamicSwitch sinks along with attached level switches:

```javascript
{
    "Serilog": {
        "Using": ["Serilog.Sinks.DynamicSwitch"],
        "LevelSwitches": {
          "$appLogLevel": "Debug",
          "$netLogLevel": "Information",
          "$sysLogLevel": "Error"
        },
        "MinimumLevel": {
          "ControlledBy": "$appLogLevel",
          "Override": {
            "Microsoft": "$netLogLevel",
            "System": "$sysLogLevel"
          }
        }
        "WriteTo": [{
            "Name": "Console"
            },
            {
                "Name": "DynamicSwitch",
                "Args": {
                    "switchName": "Application",
                    "levelSwitch": "$appLogLevel"
                }
            },
            {
                "Name": "DynamicSwitch",
                "Args": {
                    "switchName": "Microsoft",
                    "levelSwitch": "$netLogLevel"
                }
            },
            {
                "Name": "DynamicSwitch",
                "Args": {
                    "switchName": "System",
                    "levelSwitch": "$sysLogLevel"
                }
            }
        ],
        "Properties": {
            "Application": "Serilog DynamicSwitch Console Sample"
        }
    }
}
```

#### Using Dependency Injection

To support the retrieval of switches through dependency injection, there is a simple extention method to add the collection to the services container. If a collection is not passed, the global `LoggingLevelSwitchCollection.Current` will be added to the container:

```csharp
.ConfigureServices((hostContext, services) =>
{
    //...
    services.AddLoggingLevelSwitchCollection();
});
```

## License

MIT
