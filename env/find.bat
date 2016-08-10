@echo off
SETLOCAL
set curDir=%1
if "%curDir%"=="." set curDir=%cd%
for /f %%i in ('dir /b/a-h-d %curDir%\%3 2^>nul') do findstr /ilmp %2 %curDir%\%%i
for /f %%i in ('dir /b/ad-h %curDir% 2^>nul') do call %0 %curDir%\%%i %2 %3
ENDLOCAL