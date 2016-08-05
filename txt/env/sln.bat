@echo off
pushd %srcDir%
msbuild %srcDir%.csproj /t:restore /v:m
devenv %txtDir%Xamarin.Forms.CarouselView.sln