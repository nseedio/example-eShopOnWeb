@echo off

REM NSEED-vNEXT: At the moment NSeed CLI does not support the "seed" command. That's why this BAT file as a temporary workaround.

IF "%1"=="destroy" (
dotnet run --no-build --no-launch-profile -p "TemporaryYieldDestroyer\TemporaryYieldDestroyer.csproj"
) ELSE (
dotnet run --no-build --no-launch-profile -p "Seeds\Seeds.csproj" -- %*
)