@echo off
setlocal EnableDelayedExpansion EnableExtensions

call vcvarsallShim.bat

set target=%1
set config=%2
set iosServerAddress=%3
set iosServerUser=%4
set iosServerPassword=%5

call build.bat %target% %config% AnyCPU
call build.bat %target% %config% x86
:: call build.bat %target% %config% x64
:: call build.bat %target% %config% arm
call build.bat %target% %config% iPhone
call build.bat %target% %config% iPhoneSimulator

; call build.bat %target% ad-hoc iPhone
; call build.bat %target% appStore iPhone
