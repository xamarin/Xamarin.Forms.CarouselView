````
Microsoft (R) Build Engine version 14.0.25420.1
Copyright (C) Microsoft Corporation. All rights reserved.

  RECURSE [debug|mobile]: Xamarin.Forms.CarouselView -> { dotnet, monoDroid, xamarin.ios, monoTouch, uap, wpa, win }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMobileLibraryProject -> true
    DefineConstants -> TRACE;DEBUG;
  RECURSE [debug|dotnet|dotnet]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    LibraryPlatform -> dotnet
    DefineConstants -> TRACE;DEBUG;
    TargetProject -> portable
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\Portable\v4.5\Microsoft.Portable.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|dotnet]: Xamarin.Forms.CarouselView -> {  }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryProject -> true
    LibraryPlatform -> dotnet
    DefineConstants -> ANYCPU;TRACE;DEBUG;
    TargetProject -> portable
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\Portable\v4.5\Microsoft.Portable.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\dotnet\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\dotnet\
  RECURSE [debug|monoDroid|monoDroid]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> android
    LibraryPlatform -> monoDroid
    DefineConstants -> ANDROID;TRACE;DEBUG;
    TargetProject -> monoDroid
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\Android\Xamarin.Android.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|monoDroid]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> android
    LibraryPlatform -> monoDroid
    DefineConstants -> ANYCPU;ANDROID;TRACE;DEBUG;COMPOSITE;
    TargetProject -> monoDroid
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\Android\Xamarin.Android.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\monoDroid\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\monoDroid\
  BUILD [debug|anycpu|monoDroid|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> android
    LibraryPlatform -> monoDroid
    DefineConstants -> ANYCPU;ANDROID;TRACE;DEBUG;
    TargetProject -> monoDroid
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\Android\Xamarin.Android.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\monoDroid\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\monoDroid\
  RECURSE [debug|xamarin.ios|xamarin.ios]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> ios
    MobileSubPlatform -> ios.unified
    LibraryPlatform -> xamarin.ios
    DefineConstants -> __UNIFIED__;__MOBILE__;__IOS__;IOS;IOS_UNIFIED;TRACE;DEBUG;
    TargetProject -> xamarin.ios
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.iOS.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|xamarin.ios]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> ios
    MobileSubPlatform -> ios.unified
    LibraryPlatform -> xamarin.ios
    DefineConstants -> __UNIFIED__;__MOBILE__;__IOS__;ANYCPU;IOS;IOS_UNIFIED;TRACE;DEBUG;COMPOSITE;
    TargetProject -> xamarin.ios
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.iOS.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\xamarin.ios\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\xamarin.ios\
  BUILD [debug|anycpu|xamarin.ios|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> ios
    MobileSubPlatform -> ios.unified
    LibraryPlatform -> xamarin.ios
    DefineConstants -> __UNIFIED__;__MOBILE__;__IOS__;ANYCPU;IOS;IOS_UNIFIED;TRACE;DEBUG;
    TargetProject -> xamarin.ios
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.iOS.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\xamarin.ios\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\xamarin.ios\
  RECURSE [debug|monoTouch|monoTouch]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> ios
    MobileSubPlatform -> ios.classic
    LibraryPlatform -> monoTouch
    DefineConstants -> __MOBILE__;__IOS__;IOS;IOS_CLASSIC;TRACE;DEBUG;
    TargetProject -> monoTouch
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.MonoTouch.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|monoTouch]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> ios
    MobileSubPlatform -> ios.classic
    LibraryPlatform -> monoTouch
    DefineConstants -> __MOBILE__;__IOS__;ANYCPU;IOS;IOS_CLASSIC;TRACE;DEBUG;COMPOSITE;
    TargetProject -> monoTouch
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.MonoTouch.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\monoTouch\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\monoTouch\
  BUILD [debug|anycpu|monoTouch|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> ios
    MobileSubPlatform -> ios.classic
    LibraryPlatform -> monoTouch
    DefineConstants -> __MOBILE__;__IOS__;ANYCPU;IOS;IOS_CLASSIC;TRACE;DEBUG;
    TargetProject -> monoTouch
    TargetsFile -> C:\Program Files (x86)\MSBuild\Xamarin\iOS\Xamarin.MonoTouch.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\monoTouch\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\monoTouch\
  RECURSE [debug|uap|uap]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> windows
    MobileSubPlatform -> windows.universal
    LibraryPlatform -> uap
    DefineConstants -> WINDOWS;WINDOWS_UWP;TRACE;DEBUG;
    TargetProject -> uap
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|uap]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.universal
    LibraryPlatform -> uap
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_UWP;TRACE;DEBUG;COMPOSITE;
    TargetProject -> uap
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\uap\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\uap\
  BUILD [debug|anycpu|uap|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.universal
    LibraryPlatform -> uap
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_UWP;TRACE;DEBUG;
    TargetProject -> uap
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\uap\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\uap\
  RECURSE [debug|wpa|wpa]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> windows
    MobileSubPlatform -> windows.phone
    LibraryPlatform -> wpa
    DefineConstants -> WINDOWS;WINDOWS_PHONE_APP;TRACE;DEBUG;
    TargetProject -> wpa
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|wpa]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.phone
    LibraryPlatform -> wpa
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_PHONE_APP;TRACE;DEBUG;COMPOSITE;
    TargetProject -> wpa
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\wpa\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\wpa\
  BUILD [debug|anycpu|wpa|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.phone
    LibraryPlatform -> wpa
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_PHONE_APP;TRACE;DEBUG;
    TargetProject -> wpa
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\wpa\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\wpa\
  RECURSE [debug|win|win]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsMetaPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    LeafPlatform -> anycpu
    MobilePlatform -> windows
    MobileSubPlatform -> windows.tablet
    LibraryPlatform -> win
    DefineConstants -> WINDOWS;WINDOWS_APP;TRACE;DEBUG;
    TargetProject -> win
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
  BUILD [debug|anycpu|win]: Xamarin.Forms.CarouselView -> { part }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.tablet
    LibraryPlatform -> win
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_APP;TRACE;DEBUG;COMPOSITE;
    TargetProject -> win
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\win\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\win\
  BUILD [debug|anycpu|win|part]: Xamarin.Forms.CarouselView.Portable -> { dotnet }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsLeafPlatform -> true
    IsMobileLibraryPlatform -> true
    IsPartPlatform -> true
    IsMobileLibraryProject -> true
    MobilePlatform -> windows
    MobileSubPlatform -> windows.tablet
    LibraryPlatform -> win
    DefineConstants -> ANYCPU;WINDOWS;WINDOWS_APP;TRACE;DEBUG;
    TargetProject -> win
    TargetsFile -> C:\Program Files (x86)\MSBuild\Microsoft\WindowsXaml\v14.0\Microsoft.Windows.UI.Xaml.CSharp.targets
    CommonTargetsPath -> C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets
    CSharpCoreTargetsPath -> Microsoft.CSharp.Core.targets
    OutDirAbsolute -> F:\git\xam\cv\bld\bin\debug\carouselView\lib\part\win\
    IntermediateOutputPathAbsolute -> F:\git\xam\cv\bld\obj\debug\carouselView\lib\part\win\
````
