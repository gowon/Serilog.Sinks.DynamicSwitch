# Serilog.Sinks.DynamicSwitch

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Serilog.Sinks.DynamicSwitchn?color=blue)](https://www.nuget.org/packages/Serilog.Sinks.DynamicSwitch)
[![Nuget](https://img.shields.io/nuget/dt/Serilog.Sinks.DynamicSwitch?color=blue)](https://www.nuget.org/packages/Serilog.Sinks.DynamicSwitch)
![build](https://github.com/gowon/Serilog.Sinks.DynamicSwitch/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/gowon/Serilog.Sinks.DynamicSwitch/branch/master/graph/badge.svg)](https://codecov.io/gh/gowon/Serilog.Sinks.DynamicSwitch)

A Serilog sink that exposes a simple interface to retrieve and change LoggingLevelSwitches at runtime.

## Using

To get started install the *Serilog.Sinks.DynamicSwitch* package:

```powershell
PM> Install-Package Serilog.Sinks.DynamicSwitch
```

or

```bash
$ dotnet add package Serilog.Sinks.DynamicSwitch
```

To start using the DynamicSwitch, the sink can be setup as follows:

```csharp
var logLevel = new LoggingLevelSwitch(LogEventLevel.Warning);

var log = new LoggerConfiguration()
    .WriteTo.Console(levelSwitch: logLevel)
    .WriteTo.DynamicSwitch("Test", logLevel)
    .CreateLogger();
```

If using `appsettings.json` for configuration, the following example shows how to configure DynamicSwitch sinks:

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

Next, add the level switch collection to your services collection:

```csharp
.ConfigureServices((hostContext, services) =>
{
    //...
    services.AddLoggingLevelSwitchCollection();
});
```

## License

MIT
