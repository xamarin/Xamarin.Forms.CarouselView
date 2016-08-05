@echo off
setlocal EnableDelayedExpansion EnableExtensions

set androidEmulatorId=4C0496D6-53F0-470C-9B20-CF0FD4DF71C4

emulatorcmd.exe /sku:android uninstall /type:device /id:%androidEmulatorId%
emulatorcmd.exe /sku:android install /type:device /id:%androidEmulatorId%
emulatorcmd.exe /sku:android launch /id:%androidEmulatorId%
