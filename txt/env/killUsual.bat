@echo off

echo kill adb.exe
taskkill /f /im adb.exe

echo kill msbuild.exe
taskkill /f /im msbuild.exe

echo kill nunit-agent.exe
taskkill /f /im nunit-agent.exe