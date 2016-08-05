@echo off
pushd %srcDir%\carouselView
msbuild %srcDir%.csproj /t:restore /v:m
devenv %srcDir%Xamarin.Forms.CarouselView.sln