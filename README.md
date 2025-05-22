A sample NuGet package project that contains `appsettings.json` config to execute the config variables.

To create a NuGet package:
```ps
dotnet pack -c Release
```
or (if we want to ensure dependencies in the `.nupkg`):
```ps
dotnet pack --include-symbols --include-source -c Release
```

To install the package locally:
```ps
dotnet add package IntegrationsJsonPlaceHolder --source directoryOfIntegrationsJsonPlaceHolderProject/bin/Release
```

To reinstall the package locally:
```ps
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet add package IntegrationsJsonPlaceHolder --source directoryOfIntegrationsJsonPlaceHolderProject/bin/Release
```

How to use the package for your projects:

In your own `appsettings.json`, you need to add the following data:
```json
{

  "JsonPlaceHolder": {
    "KeyURL": 1
  },

}
```
`KeyURL` is the key used to get the URL from the package you want to use.