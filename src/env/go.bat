@echo off
msbuild %srcDir%.csproj /t:go /nologo /v:m /p:go=%1% & cmd.bat
