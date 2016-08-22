@echo off
pushd %~dp0

prompt $p $g

:: initialize VS environment
call vcvarsallShim.bat

set envDir=%~dp0

:: cd to root of repo
pushd %~dp0..\
set repoDir=%cd%\
set srcDir=%repoDir%src\
set extDir=%repoDir%ext\
set shimProj=%extDir%shim\shim.proj
set publishProj=%extDir%publish\publish.proj
set shellProj=%extDir%shell\shell.proj

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
set doskey=%envDir%doskey.txt
doskey /macrofile=%doskey%
color 1f

:: use unicode
chcp 65001 >NUL

:: check environment
call where git.exe  > nul 2>&1
if %errorlevel% neq 0 (echo 'git.exe' not found on path. Please add git.exe to path.)
