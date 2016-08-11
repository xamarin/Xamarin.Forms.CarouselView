@echo off
msbuild %shellProj% /t:go /nologo /v:m /p:go=%1% & cmd.bat
