@echo off
setlocal EnableDelayedExpansion EnableExtensions

adb uninstall AndroidCarouselGallery.AndroidCarouselGallery
adb install %binariesDir%debug\test\Gallery\monoAndroid\AndroidCarouselGallery.AndroidCarouselGallery-Signed.apk
:: adb shell am start -n AndroidCarouselGallery.AndroidCarouselGallery/carouselgallery.droid.MainActivity

