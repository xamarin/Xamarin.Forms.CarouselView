@echo off
setlocal EnableDelayedExpansion EnableExtensions

set config=debug

set testDir=%binariesDir%%config%\test\
set testLogName=test.log
set testErrName=test.err
set testXmlName=test.xml

set exe=UITests\android\CarouselGallery.UITest.Android.exe

pushd %testDir%

echo workingDir: %testDir%
echo %testLogName% -^> %testDir%%testLogName%
echo %testErrName% -^> %testDir%%testErrName%
echo %testXmlName% -^> %testDir%%testXmlName%

set command=nunit-console %exe% /out:%testLogName% /err:%testErrName% /xml:%testXmlName%
echo %command%
%command%

popd