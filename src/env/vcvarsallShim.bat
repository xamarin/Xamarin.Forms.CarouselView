@echo off
setlocal EnableDelayedExpansion EnableExtensions

:: Define a prefix for most output progress messages that come from this script. That makes
:: it easier to see where these are coming from. Note that there is a trailing space here.
set __MsgPrefix=VCVARSALLSHIM: 

set __VSVersion=vs2015
if /i "%__VSVersion%" == "vs2015" set __VSProductVersion=140

:: Check presence of VS
if not defined VS%__VSProductVersion%COMNTOOLS goto NoVS
set __VSToolsRoot=!VS%__VSProductVersion%COMNTOOLS!

:: Does VS really exist?
if not exist "%__VSToolsRoot%\..\IDE\devenv.exe"      goto NoVS
if not exist "%__VSToolsRoot%\..\..\VC\vcvarsall.bat" goto NoVS
if not exist "%__VSToolsRoot%\VsDevCmd.bat"           goto NoVS

:: Call vcvarsall
set __VCBuildArch=x86_amd64
REM echo %__MsgPrefix%Using environment: "%__VSToolsRoot%..\..\VC\vcvarsall.bat" %__VCBuildArch%
endlocal & call "%__VSToolsRoot%..\..\VC\vcvarsall.bat" %__VCBuildArch%

:: Success
exit /b 0

:NoVS
echo Visual Studio 2015+ ^(Community is free^) is a prerequisite to build this repository.
exit /b 1
