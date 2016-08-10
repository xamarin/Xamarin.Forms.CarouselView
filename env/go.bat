@echo off
msbuild %shimProj% /t:go /nologo /v:m /p:go=%1% & cmd.bat
