@echo off
setlocal EnableDelayedExpansion EnableExtensions

if "%1"=="" (
	echo build.bat target [cfg=debug] [plat=AnyCPU] [server] [user] [password]
	exit /b 1
)

set target=%1
set buildCfg=%2
set buildPlat=%3

if "%buildCfg%"=="" set buildCfg=debug
if "%buildPlat%"=="" set buildPlat=AnyCPU

if not "%4"=="" set "iosServerAddress=%4"
if not "%5"=="" set "iosServerUser=%5"
if not "%6"=="" set "iosServerPassword=%6"

set logDir=logs\
if not exist %logDir% mkdir %logdir%

set "buildLog=%logDir%%buildPlat%_%buildCfg%.log"
set "buildWrn=%logDir%%buildPlat%_%buildCfg%.wrn"
set "buildErr=%logDir%%buildPlat%_%buildCfg%.err"

set msbuildLogArgs=^
/fileloggerparameters:Verbosity=normal;LogFile="%buildLog%" ^
/fileloggerparameters1:WarningsOnly;LogFile="%buildWrn%" ^
/fileloggerparameters2:ErrorsOnly;LogFile="%buildErr%" ^
/consoleloggerparameters:Summary ^
/verbosity:minimal

set msbuildArgs="%target%" ^
%msbuildCommonArgs% ^
%msbuildLogArgs% ^
/p:Configuration=%buildCfg%  ^
/p:Platform=%buildPlat% ^
/nologo

if defined iosServerAddress set msbuildArgs=%msbuildArgs% /p:serverAddress=%iosServerAddress%
if defined iosServerUser set msbuildArgs=%msbuildArgs% /p:serverUser=%iosServerUser%
if defined iosServerPassword set msbuildArgs=%msbuildArgs% /p:serverPassword=%iosServerPassword%

echo build.bat %target% %buildCfg% %buildPlat% %iosServerAddress% %iosServerUser% %iosServerPassword%
echo msbuild %msbuildArgs%
msbuild %msbuildArgs%
echo.