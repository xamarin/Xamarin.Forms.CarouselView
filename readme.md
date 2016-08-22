# Quick Start
Xamarin.Forms.Carousel repo contains an alpha Xamarin Forms build environment. Similar to other .NET foundation repos (e.g. coreclr) everything starts in a shell. From there you'll need to build (or at least restore nuget packages) before opening the solution. The next generation build environment is being developed in the xfproj branch. For now the following commands should get you off the ground:

## Opening Solution

1. launch shell via `\env\env.lnk`
2. type `restore` to restore nuget files
3. type `\src\Xamarin.Forms.CarouselView.sln`

## Building like CI does

1. open `cmd.exe` \env\env.lnk
2. type `build` to build both debug and release package
3. nuget package will be at `\bin\bld\release\carouselView\nuget\`
4. the build will be archived at `\drp\number\10001\`

# Xamarin.Forms Build System
The Xamarin.Forms library build environment addresses challenges encountered while developing CarouselView with the goal of simplifying the creation of Xamarin.Forms libraries in general. 

## Highlights
This section enumerates selected achievements of this effort specific to Xamarin.Forms library creation (as opposed to general build enhancements).

Consumption of libraries is simplified. The number of binaries a Xamarin.Forms app needs to reference to use a library is reduced from 3 (portable, platform, and shim [to support the linker]) to 1. This is achieved by compiling the portable (and shim) logic into the platform library. This allows a `RenderWithAttribute` applied to the Xamarin.Forms element to directly reference the platform renderer ([see here][2]). This obviates the need for the shim library and dodges a large class of potential linker issues. A compiler error is still generated during library construction if the platform logic references internal portable logic (under the hood, this is achieved by kicking off additional compilations of the project. The code can check for the `COMPOSITE` compilation symbol to know what type of compilation is occurring).

Creation of libraries is similarly simplified. The number of C# projects required for building a library which supports all Xamarin.Forms platforms is reduced from 13 (e.g. a project for Portable, Android, iOS [classic & unified], and Windows [tablet, phone, uap] + shims) to 1. This is achieved by "merging" the 13 project files into a single project file with each merged project file becoming its own platform. So, for example, the Android CarouselView library can be built like this:

    src\carouselView\lib> msbuild myLibrary.csproj /p:platform=monodroid

To build the iOS classic version substitute `monodroid` with `monotouch`. To build all platforms at once use the group platform `mobile`. To peek under the hood, pass `/t:dryRun` (see [example](doc/dryRun.md)) to dump the tree traversal used to build these "meta-platforms". 

The number of C# projects required to build an app (as is necessary for library testing) is similarly reduced from 6 (Android, iOS [classic & unified], and Windows [tablet, phone, uap]) to 1 and also has a corresponding seta of meta-platforms.

## Platform Tree
The full "platform tree" for library, app, and test projects are shown below (compiler defines are given in brackets).

````
▌ all
├──▌ pack (references mobile)
└──▌ mobile
   ├──▌portable
   │  └──▌ dotnet (library)
   ├──▌android [ANDROID]
   │  ├──▌ monodroid (library)
   │  ├──▌ monodroid.app (app)
   │  └──▌ android.aut (automation)
   ├──▌ios [IOS]
   │  └──▌ ios.unified [IOS_UNIFIED]
   │  │  ├──▌ xamarin.ios (library)
   │  │  ├──▌ xamarin.ios.phone (app)
   │  │  └──▌ xamarin.ios.sim (app)
   │  ├──▌ ios.classic [IOS_CLASSIC]
   │  │  ├──▌ monotouch (library)
   │  │  ├──▌ monotouch.phone (app)
   │  │  └──▌ monotouch.sim (app)
   │  └──▌ ios.aut (automation)
   └──▌windows [WINDOWS]
      ├──▌ windows.universal [WINDOWS_UWP]
      │  ├──▌ uap (library)
      │  └──▌ uap.32 (app)
      ├──▌ windows.phone [WINDOWS_PHONE_APP]
      │  ├──▌ wpa (library)
      │  └──▌ wpa.32 (app)
      └──▌ windows.tablet [WINDOWS_APP]
         ├──▌ win (library)
         ├──▌ win.32 (app)
         ├──▌ win.64 (app)
         └──▌ wpa.arm (app)
````

## Solution Configuration and Platforms
Creating a new Xamarin.Forms creates platforms in the solution file (`AnyCPU`, `ARM`, `x64`, `x86`, `IPhone`, `IPhoneSimulator`) which do not map directly to what we think of as _mobile_ platforms. The unified project system creates a set of meta-platforms that do map directly to mobile platforms and these are exposed in Visual Studio.

| Solution | Library | App | Test | 
| --- | --- | --- | --- |
| Android | monotouch | monotouch.app | android.aut |
| iOS | monodroid | monodroid.phone | android.aut |
| iOS (sim) | monodroid | monodroid.sim | ios.aut |
| iOS (phone) | monodroid | monodroid.phone | ios.aut |
| iOS Unified (sim) | xamarin.ios | xamarin.ios.sim | ios.aut |
| iOS Unified (phone) | xamarin.ios | xamarin.ios.phone | ios.aut |
| Win Phone | wpa | wpa.32 | |
| Win Universal | uap | uap.32 | |
| Win Tablet (x32) | win | win.32 | |
| Win Tablet (x64) | win | win.64 | |
| Win Tablet (arm) | win | win.arm | |


## Unified Xamarin.Forms Project
The unified Xamarin.Forms library and app projects contain a folder for each platform the contents of which will only be compiled (and have correct intellisense) when the corresponding platform is specified. 

Debugging support is gently hacked in with the addition of CarouselView.App.[Platform] C# projects. These projects could, should, and hopefully will, be merged into the unified app project via the creation of a VSIP plugin. 

Reflecting over project settings in Visual Studio has been made to work (e.g. when Properties is opened the compilation symbols will be correct) however edits to the unified project via the UI are generally not supported (e.g. adding a new file for a specific platform via the UI will require editing the project file by hand and moving the Compile element under the [ItemGroup][3] with the correct Condition for the platform in which it's expected to be compiled). Again, a VSIP plugin would make this experience more "delightful".

As viewed from the Visual Studio Solution Explorer the unified project for CarouselView is the following:

````
▌ CarouselView
├──▌ CarouselView.csproj
│  ├──▌ Properties
│  │  └─ AssemblyVersion.cs - Contains AssemblyVersion attribute
│  │     └─ AssemblyVersion.t.cs - Template expaded with msbuild variables; build fails if expansion doesn't match
│  ├──▌ Portable
│  │  └─ AssemblyInfo.cs - Typical AssemblyInfo.cs minus AssemblyVersion attribute
│  ├──▌ Android [ANDROID]
│  ├──▌ iOS [IOS]
│  └──▌ Windows [WINDOWS]
│
├──▌ CarouselView.App.csproj
│  ├──▌ Properties
│  │  └─ AssemblyVersion.cs
│  │     └─ AssemblyVersion.t.cs
│  ├──▌ Portable
│  │  └─ AssemblyInfo.cs
│  ├──▌ Android [ANDROID]
│  ├──▌ iOS [IOS]
│  ├──▌ Windows [WINDOWS]
│  │  ├──▌ Phone [WINDOWS_PHONE_APP]
│  │  ├──▌ Tablet [WINDOWS_APP]
│  │  └──▌ Universal [WINDOWS_UWP]
│  ├──▌ Resources (Xamarin Android project insists, for the moment, it's resource directory live at the project root)
│  ├─ Info.plist (Xamarin iOS project insists, for the moment, they live at the project root)
│  └─ Entitlements.plist (Xamarin iOS project insists, for the moment, they live at the project root)
│
├──▌ debugger - make one of the following projects current to enable debugging of CarouselView.App
│  ├──▌ CarouselView.App.Android
│  ├──▌ CarouselView.App.iOS
│  ├──▌ CarouselView.App.Windows.UAP
│  ├──▌ CarouselView.App.Windows.Phone
│  └──▌ CarouselView.App.Windows.Tablet
│
└──▌ CarouselView.Test
````

# General Build System

## Directories
The following directories are well known to the build system and shell. Navigate to most well-known directories by typing its name into the shell. Type `r` to navigate to the root directory.

### Enlistment
The following well-known directories are under source control. Build artifacts are placed outside of these directories which allows for a vastly simplified [.gitignore](.gitignore) file.
````
▌ root of enlistment
├──▌ src – files that contribute to build output
├──▌ ext - files that extend msbuild
├──▌ env - files that initialize the shell; misc .bat files
└──▌ doc - files that document the system
````

### Artifacts
The following well-known directories are under generated by the build and are not under source control.
````
▌ root of enlistment
├──▌ bld - output of build
│  ├──▌ bin - resultant binaries of build
│  └──▌ obj - temporary build files
├──▌ dls - downloads; cache of files needed before going offline
│  ├──▌ packages - nuget packages
│  └──▌ tools - misc tools; nuget.exe
└──▌ drp - archive of builds kicked off from root
````

## Sub-Directories
Sub-directories of well-known directories. They generally do not have shell aliases. The following have structure specific to their function worth documenting.

### /bld/
Build output generated from a sub-directory in `src` is placed in the same sub-directory in `bin` (or `obj`). For example, build output from building `src\carouselView\lib\CarouselView.csproj` is placed in `bld\bin\carouselView\lib` and `bld\obj\carouselView\lib`. 

If projects are built using the shim (a project which searches for and then builds .csproj files after, among other things, configuring logging. It lives at ext\shim\shim.proj which and is typically invoked through its alias `build`, `rbuild`, `dbuild`) then log files of all verbosities (summary, normal, detailed, and diag) and severities (warning, and error) are also created. If the shim is invoked from the root then it is assumed the build will be published (copied) to `drp\number\$(BuildNumber)` and a build number is generated and recorded along with the current source control identifying information (revision, branch, url, etc).
````
▌ bin
└──▌ bld
│  ├─ BuildInfo.Build.Number - computed by adding one to the last sub-directory of drp\number\
│  ├─ BuildInfo.Enlistment.revision - computed by consulting source control (git)
│  ├─ BuildInfo.Enlistment.status - capture the state of the enlistment; publication fails if dirty
│  ├─ Shim.Build.*.log - log of shim launching other build processes
│  ├─ Core.NugetRestore.*.log - log of nuget restore which happens before launching core build
│  └──▌ debug or release
│     ├─ Core.Build.*.log - compiler errors will appear in here
│     └──▌ [relative directory from src]
└──▌ obj
   └──▌ debug or release
      └──▌ [relative directory from src]
         └─ BuildInfo.cs - generated by shim, replaces universally included src\BuildInfo.cs during build
````

### /drp/
Successful builds of clean enlistments from the root using the shim (see [bld](#bld)) will have their `bin\bld` directory copied to `drp\number\$(BuildNumber)` where the build number is a function of the subdirectories found in `drp\number`. For example, if `drp\number` has sub-directories `10000`, `10001` and `10002` then the next build build number would be `10003`. A symlink is also created at `drp\revision\$(EnlistmentRevision)` which points to the corresponding build. For example, in git an enlistment revision would be the git hash. 
````
▌ drp
├──▌ number - archived builds by build number
└──▌ revision - archived builds by source control revision
````

## Projects
Project files have been modified to enable build features that simplify maintaining a CI infrastructure at the expense of tooling. The main tooling break are design time features of Visual Studio which modify project files because, without VSIP integration, Visual Studio in unaware of the new conventions. For example, adding a new file Android specific file via Visual Studio to [`CarouselView.csproj`][2] will require moving the `<Compile Include="NewAndroidFile.cs">` element to live under `<ItemGroup Condition=" '$(MobilePlatform)' == '$(AndroidMobilePlatformId)'" >`. 

Until VSIP integration is available, manual edits are more easily made after installing [EditProj][1]. For more extensive edits, unload all projects under `src` and open project files from the shared project (`.repo`)[.repo]. The `.repo` project includes all msbuild files which allows for global search and replace of msbuild symbols. The shell alias `ts` will touch the solution file which has the effect of reloading changes made to msbuild files which are included by project files (which are otherwise cashed once at startup and never refreshed).

### General Project Template
Project files all conform to the following general template sections of which are described in subsequent sections.

```xml
<Project>
  <PropertyGroup>
    <!--Properties that identify the type of the project-->
  </PropertyGroup>
  
  <!--Load properties that are a function of the type of the project-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildProjectDirectory)\, .pre.props))\.pre.props" />

  <!--Load common properties for the given type of the project-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .props))\.props" />

  <PropertyGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--Set properties specific to this project; the goal is to have as few such properties as possible-->
  </PropertyGroup>
  <ItemGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--Project Files-->
    <!--Project References-->
  </ItemGroup>
  
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .targets))\.targets" />
</Project>
```

### Common Properties
Typically, solutions with multiple projects suffer from duplication of project settings. For example, to enable warnings as errors typically requires modifying each project. To prevent duplication and allow settings to be centrally administered common project settings are extracted to `.props` files in parent directories. For example, `WarningLevel` and `TreatWarningsAsErrors` have been extracted to [`src\.props`](src/.props) and so are included by all projects in any subdirectory of `src`. 

Those `.props` files "closest" to the project override those files further away. For example, the [`.props`](.props) file in root directory is included before any other simultaneously making its definitions available to those files and allowing them to override those settings. For example, the `.props` files processed when loading [`CarouselView.csproj`][2] are, [`\.props`](.props), then [`src\.props`](src/.props), then [`src\carouselView\.props`](src/carouselview/.props), then finally the project itself. Note that they are included in the reverse order but, because the first thing they each do is import their parent `.props` file they are logically processed in the reverse of the include order. Don't think too hard about it; It works as you'd expect.

### Project and Platform Types


# Shell

## Shell Commands

### Shim
```
build - build both release and debug
rbuild - build release
dbuild - build debug
clean - clean both release and debug
rclean - clean release
dclean - clean debug
```

[1]: https://visualstudiogallery.msdn.microsoft.com/b346d9de-8722-4b0e-b50e-9ae9add9fca8
[2]: src/carouselView/lib/CarouselView.csproj
