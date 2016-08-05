@echo off
pushd %~dp0

prompt $p $g

:: initialize VS environment
call vcvarsallShim.bat

:: cd to root of repo
pushd %~dp0..\..
set repoDir=%cd%\

:: directories
set drpDir=%repoDir%drp\
set binDir=%repoDir%bin\
set tstDir=%repoDir%tst\
set bldDir=%binDir%bld\
set tmpDir=%binDir%tmp\

set dlsDir=%repoDir%dls\
set toolsDir=%dlsDir%tools\
set packagesDir=%dlsDir%packages\

set txtDir=%repoDir%txt\
set envDir=%txtDir%env\
set nuspecDir=%txtDir%nuspec\
set nugetDir=%txtDir%nuget\
set srcDir=%txtDir%src\
set devDir=%srcDir%dev\
set testDir=%srcDir%test\

:: variables
set platform=

:: defaults
set androidEmulatorDefault=4C0496D6-53F0-470C-9B20-CF0FD4DF71C4

:: paths
set path=%path%;%envDir%
set path=%path%;%ProgramFiles(x86)%\MSBuild\Xamarin\Android
set path=%path%;%ProgramFiles(x86)%\Windows Kits\10\Debuggers\x64
set path=%path%;%ProgramFiles(x86)%\Android\android-sdk\platform-tools
set path=%path%;%ProgramFiles(x86)%\Android\android-sdk\tools
set path=%path%;%ProgramFiles(x86)%\Microsoft Emulator Manager\1.0
set path=%path%;%ProgramFiles%\Perforce
set path=%path%;%packagesDir%NUnit.Runners.2.6.4\tools\

:: super paths
set path=%toolsDir%;%path%
set path=%nuget3%;%path%

:: alises
set alias=%envDir%alias.txt
doskey /macrofile=%alias%
color 1f

:: use unicode
chcp 65001 >NUL
