@ECHO OFF
REM NOTE: This file was auto-generated with `IB.exe prepare` from `IntelliFactory.Build`.

setlocal
rd /s /q tools\packages
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\4.1\Framework\v4.0
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\4.0\Framework\v4.0
set PATH=%PATH%;tools\NuGet
nuget install IntelliFactory.Build -nocache -pre -ExcludeVersion -o tools\packages
nuget install FSharp.Compiler.Tools -nocache -version 4.0.1.21 -excludeVersion -o tools/packages
tools\packages\FSharp.Compiler.Tools\tools\fsi.exe --exec build.fsx %*
