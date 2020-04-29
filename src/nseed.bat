@echo off
REM NSEED-vNEXT: At the moment NSeed CLI does not support the "seed" command. Thats why this bat file and this temporary workaround.
dotnet run --no-build --no-launch-profile -p "Seeds\Seeds.csproj" -- %*