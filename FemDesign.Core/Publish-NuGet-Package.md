# Publish NuGet package
Publish the NuGet package to nuget.org 
1. Update `.nuspec` info
2. Run `nuget pack` (Requires `nuget.exe` in PATH)
3. Run `nuget push <NUPKG> <API_KEY> -Source https://api.nuget.org/v3/index.json`
