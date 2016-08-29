# Quick Start
Xamarin.Forms.Carousel repo contains an alpha Xamarin Forms build environment. Similar to other .NET foundation repos (e.g. coreclr) everything starts in a shell. From there you'll need to build (or at least restore nuget packages) before opening the solution. The next generation build environment is being developed in the xfproj branch. For now, the following commands should get you off the ground:

## Opening Solution

1. open shell using `\env.lnk`
2. type `restore` to restore nuget files
3. in `src\build` type `build` (to build custom msbuild tasks)
3. open `\src\Xamarin.Forms.CarouselView.sln`

## Building like CI does

1. configure machine so that a vanilla Xamarin.Forms project builds
2. open shell using `\env.lnk`
3. type `build` to build both debug and release package
4. nuget package will be at `\bin\bld\release\carouselView\nuget\` and the build will be archived at `\drp\number\10001\`

# Xamarin.Forms Build System
The Xamarin.Forms build system addresses challenges encountered while developing CarouselView with the goal of simplifying design, build, test, and packaging of Xamarin.Forms libraries. 

The Xamarin.Forms build systems aims to do at design time what Xamarin.Forms has already done at runtime. At runtime, Xamarin.Forms abstracts the different types of mobile platforms by allowing virtual controls to be added to a virtual device which Xamarin.Forms then renders for each specific platform. At design time, the Xamarin.Forms build system aims to abstract the different types of mobile projects by merging them into a single project from which the build system generates platform specific assemblies.

On demand releasing is also a goal of Xamarin.Forms build. In practice, this means having the ability to take a machine with a freshly installed OS and in a single command (possibly powershell now that it's cross platform), install Visual Stuido, install Xamarin Studio, install 3ed party tools (git, nuget, etc), download the source, clean, restore, build, package, publish, deploy, and test on all platforms. This works focuses on the clean, restore, build, package, and publish steps. 

The documentation is dividied into [Highlighs](#highlighs) of the build system specific to Xamarin.Forms and [General Build System](#general-build-system) which describes the numerous more general build enhancements on top of which the new Xamarin.Forms build system is built:

- [`Shell`](#shell) - Establish an isolated environment from which to launch CI builds and to share aliases.
- [`Directories`](#directories) - Establish separate directories for source, downloads, build artifcats, and build archives.
- [`Projects`](#projects) - Establish convention for extracting and cenralizing [common project properties](#common-properties) (e.g. `TreatWarningsAsErrors`) and for establishing [project and platform types](#project-and-platform-types)
- [`Shim`](#shim) - Allow defining an `InitialProject` and `FinalProject` to run before and after a poset of out-of-proc builds (vs in-proc msbuild tasks). Allow defining a `Shim` project (also run in its own process) before the main build process. Enable all manner of logging on a build process and establish names for all log files.
- [`Build Identity`](#build-identity) - Establish if an enlistment is clean and, if so, define `BuildNumber`, `EnlistmentRevision`, `EnlistmentUrl`, and `EnlistmentBranch`.
- [`C# Tempaltes`](#csharp-templates) - Allow defining `Tempalte` project items to injecting msbuild variables into source code or to fail the build if a checked in expansion does match the build time expansion (e.g. used to inject `AssemblyVersion`).
- [`Publishing`](#publishing) - Automatic archiving of builds by `BuildNumber`, `EnlistmentRevision`, `EnlistmentBranch`, and `AssemblyVersion` as well as erasure of old archives. 
- [`Cleaning`](#cleaning) - Redefines `Clean` target to use source control to erase non-enlisted files (e.g. `git clean`).
- [`NugetRestore`](#nugetrestore) - Move `project.config` metadata into msbuild files as `NugetReference` and `NugetPacakge`. Add a `NugetRestore` target to download the packages.
- [`Pack`](#pack) - Move `*.nuspec` metadata into msbuild files (e.g. `NuspecAuthors`, `NuspecOwners` etc), generate a nuget package at build, and verify that a simple project can be built after upgrading to the new package.
- [`MetaPlatform`](#metaplatform) - Allow consolidation of many `.csproj` files into a single `MetaProject` containing many `MetaPlatforms` (e.g. `android`, `ios`) and `SelfReferences` in place of `ProjectReferences`.
- [`PartPlatform`](#part-platforms) - Allow merging projects (parts) into a single file (composite) while still maintaining the visibility boundries of the separate projects (e.g. generating compiler errors if a part references non-puplic members of another part).

## Highlights
Consuming and producing Xamarin.Forms libraries is simplified by:
- [reducing the number of projects](#project-reduction) consumed and produced by both mobile apps and libraries
- by the introduction of a [mobile meta-platform tree](#mobile-metaplatform-tree) which integrates with existing Visual Studio [solution](#mobile-solution) and [project](#mobile-project) tooling, and finally,
- by "de-duplicating" the msbuild files so build setting like `TreatWarningsAsErrors` appear only once and so can be centrally administered.

### Project Reduction
The number of binaries a Xamarin.Forms app needs to reference to use a Xamarin.Forms library is reduced from 3 (portable, platform, and shim [to support the linker]) to 1. This is achieved by compiling the portable and shim logic into the platform library. This allows a `RenderWithAttribute` applied to the Xamarin.Forms element to directly reference the platform renderer ([see here][2]). This obviates the need for the shim library and dodges a large class of potential linker issues. A compiler error is still generated during library construction if the platform logic references internal portable logic (note that until VSIP integration happens, Intellisense will not complain about such references). Under the hood, this is achieved by kicking off additional compilations of the project. The code can check for the `COMPOSITE` compilation symbol to know what type of compilation is occurring (see [Part Platforms](#part-platforms)). 

The number of projects required for building a Xamarin.Forms library is reduced from 13 (portable, Android, iOS [classic & unified], and Windows [tablet, phone, uap] + shims) to 1. This is achieved by "merging" the 13 project files into a single project file with each merged project file becoming its own `MetaPlatform`. So, for example, the Android CarouselView library can be built like this:

    src\carouselView\lib> msbuild /p:MetaPlatform=monodroid

To build the iOS classic version substitute `monodroid` with `monotouch`. To build all platforms at once use the group `MetaPlatform` `mobile`. 

The number of projects required to build a Xamarin.Forms app for all supported platforms is reduced from 6 (Android, iOS [classic & unified], and Windows [tablet, phone, uap]) to 1 and also has a corresponding set of `MetaPlatforms`.

### Mobile MetaPlatform Tree
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

### Mobile Solution
Typically, creating a new Xamarin.Forms project creates platforms in the solution file which do not map directly to what we think of as _mobile_ platforms (e.g. `AnyCPU`, `ARM`, `x64`, `x86`, `IPhone`, `IPhoneSimulator`). The unified project system creates a set of solution `MetaPlatforms` that do map directly to project `MetaPlatforms` list above and these are exposed in Visual Studio.

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


### Mobile Project
The unified Xamarin.Forms library and app projects contain a folder for each platform the contents of which will only be compiled (and have correct intellisense) when the corresponding platform is specified. 

Debugging support is gently hacked in with the addition of CarouselView.App.[Platform] C# projects. These projects could, should, and hopefully will, be merged into the unified app project via the creation of a VSIP plugin. 

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
Creating a build system that makes CI "delightful" requires a number of changes to the out-of-the-box templates created by Visual Studio. Documentation of these enhancements proceeds by describing (1) Build Outputs; The directories and binaries generated by a CI build, (2) Build Inputs; The project and other msbuild files that describe the build, and (3) Piecemeal Building; Decomposing the CI build into a pipeline of smaller builds that can be launched from the shell.

## Shell
The shell allows the build to not depend on Visual Studio and solution files and instead depend solely on msbuild.exe and project files.

Files supporting the creation of the shell live in [`ext/shell/`](/ext/shell/) and are listed below. A link to open the shell lives at `\env.lnk` and is initialized with [`env.bat`](ext/shell/env.bat). Upon opening, the shell will issue warnings if various preconditions are not met (e.g `git.exe` not on path). Next, the shell will initialize the Visual Studio shell environment using the [`vcvarsallShim.bat`](ext/shell/vcvarsallShim.bat). Finally, msbuild variables are imported using [`shell.proj`](ext/shell/shell.proj) and aliases are loaded from [`doskey.txt`](ext/shell/doskey.txt).
````
▌ shell
├─ env.lnk - link to launch shell
├─ env.bat - shell environment variables
├─ doskey.txt - aliases
├─ shell.proj - dumps msbulid properties so they are available to the shell (prevents duplicating msbuild values in a .bat file)
└─ vcvarsallShim.bat
````

### Shell Aliases
Use the following aliases to build and clean all projects in debug, release or both. For example, type `build` from the root to build everything (the [bld](#bld) section describe the output of `build` and [building](#building) section describe how it can be decomposed). The aliases `cv`, `lib`, `app`, `aut` will jump to the CarouselView root, library, test app, and automation test directories. Here is a subset of aliases to start with:
```
rbuild, debuild, build - build release, debug, or both
rclean, dclean, clean - clean release, debug, or both
la - list all aliases
fa - find an alias with a given string (e.g. fa carousel)
r, src, cv, lib, app, aut - navigate to common directories used in documentation examples
gs, ga - git status, add
nuke, killusual - use git to clean out directory and sub directories, kill processes holding nukable files
., .., ... - navigate up 1, 2 or 3 directories
```

## Directories
The following directories are well known to the build system and shell. Navigate to most well-known directories by typing its name into the shell. Type `r` to navigate to the root directory.

### Enlistment
The following well-known directories are under source control. Build artifacts are placed outside of these directories which allows for a vastly simplified [`.gitignore`](.gitignore) file. Note, [git-lfs](https://git-lfs.github.com/) is required to restore the binary files in `doc`.
````
▌ root of enlistment
├──▌ src – files that contribute to build output
├──▌ ext - files that extend msbuild
└──▌ doc - files that document the system
````

### Artifacts
The following well-known directories are generated by the build and are not under source control.
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
The following sub-directories of well-known directories have structure specific to their function worth documenting. Generally, sub-directories of well-known directories do not have shell aliases. 

### dls
Downloaded files are kept here. Principly, this includes nuget packages and boostraping tools.
````
▌ dls
├──▌ packages - nuget packages
│  └──▌ meta - contains packages.config files generated and used by build during package restore
└──▌ tools - boostraping tools (nuget.exe)
````

### bld
Build output is kept separate from source code; Build output generated by a project in sub-directory of `src` is placed in the same sub-directory in `bld\bin` and `bld\obj`. For example, build output from building `src\carouselView\lib\CarouselView.csproj` is placed in `bld\bin\carouselView\lib` and `bld\obj\carouselView\lib`. 

If projects are built using the shim (see [shim](#shim)) then log files of all verbosities (summary, normal, detailed, and diag) and severities (warning, and error) for all sub-builds (e.g. Shim.Build, Core.NugetRestore, Core.Build) are also created.

If the build is identifiable (see [drp](drp)) then metadata identifying the build (number, revision, branch, url) will be stored in `BuildInfo.*` files under `bin\bld` and `BuildInfo.cs` under `bld\obj\$(Configuration)`.
````
▌ bld
└──▌ bin
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

### drp
A build of a clean enlistment from the root directory via the alias `build`, successful or not, will copy the `bin\bld` directory to `drp\number\$(BuildNumber)` (see [Publishing](#publishing)). A simlink to that directory is created at `drp\revision\$(EnlistmentRevision)` and created (or overridden) at `drp\version\$(BuildVersion)`. 
````
▌ drp
├──▌ number - archived builds by build number
├──▌ revision - archived builds by source control revision  (symlinks into drp\number)
└──▌ version - archived builds by version (symlinks into drp\number)
````

## Projects
When abstracting multiple platforms, as does Xamarin.Forms, the solution file can quickly become unwieldy as more platforms are supported. For example, to build a CarouselView library without the following improvements requires dozens of projects (see [Project Reduction highlight](#project-reduction)). Most of the projects are shims contain little logic of their own. These project clutter the solution and their common settings are hard to administer. By merging these many shim projects and extracting common settings to a single location sanity is restored.

### Common Project Properties
Typically, solutions with multiple projects suffer from duplication of project settings. For example, to enable warnings as errors typically requires modifying each project. To prevent duplication and allow settings to be centrally administered common project settings are extracted to `.props` files in parent directories. For example, `WarningLevel` and `TreatWarningsAsErrors` have been extracted to [`src\.props`](src/.props) and so are included by all projects in any sub-directory of `src`. 

Those `.props` files "closest" to the project override those files further away. For example, the [`.props`](.props) file in root directory is included before any other simultaneously making its definitions available to those files and allowing them to override those settings. For example, the `.props` files processed when loading [`CarouselView.csproj`][2] are, [`\.props`](.props), then [`src\.props`](src/.props), then [`src\carouselView\.props`](src/carouselview/.props), then finally the project itself. Note that they are included in the reverse order but, because the first thing they each do is import their parent `.props` file they are logically processed in the reverse of the include order. Don't think too hard about it; It works as you'd expect.

### Types of Common Project Properties
Some settings gathered into `.props` files are common to all projects (e.g. `WarningLevel` and `TreatWarningsAsErrors`) however most are common to a specific _type_ of project, platform, or configuration. When these type specific properties are centralized into a `.props` file they are declared conditionally on whether the project, platform, or configuration is of a matching type. For example, here is a snippet of [`src/.props'](src/.props) which contains setting specific to the `debug` configuration:
````
  <!--debug-->
  <PropertyGroup Condition="'$(Configuration)'=='debug'">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <DefineConstants>$(DefineConstants)TRACE;DEBUG;</DefineConstants>
  </PropertyGroup>
````
While there are only two types of configuration (`debug` and `release`) many project and platform types have been introduced and are described below.

### MetaProject
A `MetaProject` is an amalgam of primitive project templates (primitive project templates being those that existed before this work) and has the following general form:

```xml
<Project>
  <PropertyGroup>
    <!--A MetaProject is identified by a MetaProjectGuid-->
    <MetaProjectGuid>...</MetaProjectGuid>
  </PropertyGroup>
  
  <!--Load properties with descriptive names for the type of project and platform being loaded-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildProjectDirectory)\, .pre.props))\.pre.props" />

  <!--Load common properties for the given type of the project and platform-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .props))\.props" />

  <PropertyGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--Set properties specific to this project and\or platform-->
    <!--the goal is to have as few such properties as possible-->
  </PropertyGroup>
  <ItemGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--files and references-->
  </ItemGroup>
  
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .targets))\.targets" />
</Project>
```

For example, Xamarin.Forms build defines the following `MetaProjects` for
- all Xamarin.Forms library projects
- all Xamarin.Forms app projects
- Calabash Android and iOS UI automation projects

A `.props` file can determine the type of `MetaProject` by comparing `MetaProjectGuid` to a constant however that is not very readable. Instead, `.props` files typically use properties with descriptive names declared in a hierarchy of `.pre.props` files loaded before the `.props` files. For example, the Xamarin.Forms [`xf.pre.props`](ext/xf/xf.pre.props) file defines `MetaProjectName` with values of either `xf.lib`, `xf.app`, or `xf.aut` as well as `IsMobileLibraryProject`, `IsMobileAppProject`, or `IsMobileTestProject` depending on the type of `MetaProject` being loaded. 

### MetaPlatform
`MetaPlatform` is the heart of the type system; The `MetaPlatform` abstraction allows deriving new platforms from the primitive platforms (primitive platforms being those platforms that existed before this work). For example, in the case of Xamarin.Forms, `MetaPlatform` allows for creation of `monotouch.sim`, `android`, or `win.arm` platforms out of `IPhoneSimulator`, `AnyCPU`, or `arm` primitive platforms. Each `MetaProject` supports a set of `MetaPlatforms`. For example, the `MetaPlatform` members for Xamarin.Forms `MetaProjects` are as follows:

| `MetaPlatform` | `MetaProject` |
| --- | --- |
| `xf.aut` | `android.aut`, `ios.aut` |
| `xf.lib` | `dotnet`, `monodroid`, `monotouch`, `xamarin.ios`, `win`, `uap`, `wpa` |
| `xf.app` | `monodroid.app`, `monotouch.phone`, `monotouch.sim`, `xamarin.ios.sim`, `xamarin.ios.phone`, `win.32`, `win.64`, `win.arm`, `uap.32`, `wpa.32` |

`MetaPlatforms` come in three flavors discriminated by the `MetaPlatofrmType` property values `augmented`, `proxy`, and `group` or by `IsAugmentedPlatform`, `IsProxyPlatform` and `IsGroupPlatform`. 

#### Augmented MetaPlatform
An `augmented` `MetaPlatform` is a primitive platform augmented with additional properties that more fully describe the platform type which are used by `.props` files use to declare properties common to that type. For example, the classic and unified Xamarin.Forms iOS app projects are aggregated into the `xf.app` `MetaProject` which supports two augmentations of the `IPhoneSimulator` platform discriminated by the `MetaPlatform` property values `xamarin.ios.sim` and `monotouch.sim`. Both augmentations cause the [`xf.pre.props`](ext/xf/xf.pre.props) file to declare `MobilePlatform` with the constant value of `ios` (aka `IosMobilePlatformId`) which will be used in [src\.props](src/.props) to declare the `MtouchSdkVersion`. The `xamarin.ios.sim` `derived` `MetaPlatform` can be built from the command line like this (see also: [Building MetaPlatforms](#building-metaplatforms)):

    src\carouselView\app> msbuild /v:m /p:Platform=IPhoneSimulator /p:MetaPlatform=xamarin.ios.sim

#### Proxy MetaPlatform
A `proxy` `MetaPlatform` infers its `Platform` from its `MetaPlatform` (e.g. `IPhoneSimulator` from `monotouch.sim`) and then _explictly_ passes that `Platform` to a recursive call of itself. It exists for two reasons: 

1. it provides a slighly simpler command line build syntax. For example, `xamairn.ios.sim` can be built without specifying `Platform` and only specifying `MetaPlatform` (see also: [Building MetaPlatforms](#building-metaplatforms)):

    src\carouselView\app> msbuild /v:m /p:MetaPlatform=xamarin.ios.sim
    
2. it allows `MetaPlatform` to be passed as `Platform` (unrecognized `Platforms` are assumed to be `MetaPlatforms`) which allows passing `MetaPlatforms` from Visual Studio which allows building from Visual Studio:

    src\carouselView\app> msbuild /v:m /p:Platform=xamarin.ios.sim

#### Group MetaPlatform
A `group` `MetaPlatform` is a collection of one or more `MetaPlatforms`. Groups can be named (e.g. `ios.unified` := { `xamarin.ios.phone`, `xamarin.ios.sim` }) or specified ad-hoc on the command line (e.g. `/p:MetaPlatform=monotouch.xim;xamarin.ios.sim`). When built, a `group` `MetaPlatform` will build all of its members. For example, either of the following will build both `xamarin.ios.phone` and `xamarin.ios.sim` (see also: [Building MetaPlatforms](#building-metaplatforms)):

    src\carouselView\app> msbuild /v:m /p:Platform=xamarin.ios.phone;xamarin.ios.sim
    src\carouselView\app> msbuild /v:m /p:Platform=ios.unified
    
#### MetaPlatform Hierarchy
Here is a summary of the relationships between `MetaProjects`, `MetaPlatforms`, and `MetaPlatformTypes` for Xamarin.Forms. These relationships are declared in [`ext\xf\xf.pre.props`](ext/xf/xf.pre.props).

![Platform Image](doc/Platforms.gif)

### Editing Projects
Project files have been modified to enable build features that simplify maintaining a CI infrastructure at the expense of tooling. The main expense (breaks) are design time features of Visual Studio which modify project files because, without VSIP integration, Visual Studio in unaware of the new conventions. For example, adding a new file Android specific file via Visual Studio to [`CarouselView.csproj`][2] will require moving the `<Compile Include="NewAndroidFile.cs">` element to live under `<ItemGroup Condition=" '$(MobilePlatform)' == '$(AndroidMobilePlatformId)'" >`. 

Manual edits of project files are more easily made after installing [`EditProj`][1]. For more extensive edits, unload all projects under `src` and open project files from the shared project [`.repo`](.repo.shproj). The `.repo` project includes all msbuild files. This allows for global search and replace of msbuild symbols. The shell alias `ts` touches the solution file which has the effect of reloading changes made to msbuild files which are included by project files which are otherwise cashed once at startup and never refreshed.

## Nuget
Nuget packages are restored via a new `msbuild` target `NugetRestore` (see [dls\packages\meta](#dls)). The target internally downloads and invokes `nuget.exe` on `package.config` generated from `NugetReference` and `NugetPackage` `ItemGroups`. 

## Restoring, Building, And Cleaning
The following documents the various ways to invoke `msbuild` on [`MetaProjects`](#metaproject) to restore nuget packages, build binaries, and clean up.

### Cleaning
The build can be cleaned in the following ways:

1. `msbuild /t:clean` has been overridden in `MetaProjects` to use git to remove all untracked files in the `bin` and `obj` directories for the given project.
2. the `clean` alias (similar to the `build` alias) has the effect of running `msbuild /t:clean` on all `.csproj` files in the current directory, all its sub-directories, and all projects referenced by that project set for both `debug` and `release`.
3. by manually deleting the [`bld`](#bld) directory (and optionally the [`dls`](#dls) and [`drp`](#drp) directories). 
4. the `nuke` alias which uses git to remove all untracked files in the current directory and its sub-directories returning the repo to a virgin state. 

### Restoring
Nuget packages are restored via a new `MetaProject` `msbuild` target `NugetRestore` (see [Nuget](#nuget)). Nuget packages can be restored in the following ways:

1. `msbuild /t:NugetRestore /p:SkipNugetImports` will restore all nuget packages for a given project and projects it references.
2. the `restore` alias (similar to the `build` alias) has the effect of running `msbuild /t:NugetRestore` on all `.csproj` files in the current directory, all its sub-directories, and all projects referenced by that project set. 

#### Task Assembly
In addition to restoring nuget packages before building any Xamarin.Forms `MetaProjects`, the `Xamarin.Forms.Build.dll` must also be built as this assembly contains msbuild tasks used by Xamarin.Forms `MetaProject` builds. Failure to do this will result in the following (or similar) error: 

    error MSB4036: The "RegexMatches" task was not found.

Building the task assembly is handled automatically by the `build` alias but otherwise, if not previously built or if erased after a `nuke`, the assembly must be built manually via `src\build> build`. This wrinkle will be ironed out once the task assembly is put into its own nuget file but managing a task assembly can be a general challenge when maintaining a complex build as is the case here.

### Building
Building `MetaProjects` can be done in the following ways:

1. `msbuild` will build all binaries for a given `MetaProject`. Before building this way, the nuget packages for the project will have to be restored using an approach listed in [Restoring](#restoring).
2. the `build` alias has the effect of restoring nuget packages for on all `.csproj` files in the current directory, all its sub-directories, and all projects referenced by that project set and then building that same project set.

## Building MetaPlatforms 
Any msbuild `MetaPlatform` target (e.g. `Clean`, `Build`, `NugetRestore`, etc) can act over a specific `MetaProject` by passing `/p:MetaPlatform=[MetaPlatform]`. Note, before building any of the following examples be sure to restore nuget packages for the target project using an approach listed in [Restoring](#restoring).

For example, this will build the `xamarin.ios` Xamarin.Forms `MetaPlatform` for CarouselView:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=xamarin.ios
    
In order to support Visual Studio, an unrecognized `Platform` is assumed to be a `MetaPlatform`. For example, the following is equivalent to the previous example:

    src\carouselView\lib> msbuild /v:m /p:Platform=xamarin.ios

Multiple `MetaPlatfoms` can be specified ad-hoc and built at the same time. For example, this will build `xamarin.ios` and `monotouch` platforms at the same time:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=xamarin.ios;monotouch
    
Common groups of `MetaPlatforms` have been assigned names (see [MetaPlatform Hierarchy](#metaplatform-hierarchy)). For example, the following is equivalent to the previous example:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=ios
    
Groups of `MetaPlatforms` can also be built at the same time. For example, this will build `android`, `ios`, and `windows` platforms at the same time:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=android;ios;windows
    
Common groups of groups of `MetaPlatforms` have also been assigned names. For example, the following is equivalent to the previous example:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=mobile
    
`MetaPlatforms` can reference `MetaPlatforms` within the same `MetaProject`. For example, the `MetaPlatform` `pack`, which creates and verifies a nuget package, references `mobile` so building `pack` will also build `mobile` (setting `SkipPackageCheck=true` disables the package verification build (See [nuget](#nuget)).:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=pack /p:SkipPackageCheck=true

The ur group `all` will build all `MetaPlatforms` in a `MetaProject`. For example, the following will build (and package) all binaries that compose the CarouselView nuget package:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=all /p:SkipPackageCheck=true
    
If no `MetaPlatform` is passed, then `all` `MetaPlatform` is assigned as a default. 

## Building Leaf and Part Platforms
`LeafPlatforms` and `PartPlatforms` are platforms that delegate to one of the projects that compose the `MetaProject` to actually preform the build. 

### Leaf Platforms
A `ProxiedPlatform` is a desktop platform augmented with a `meta` `MetaPlatform` (see [MetaPlatform](#metaplatform)). Every `ProxiedPlatform` has no children and one or more parent `MetaPlatforms` (see [MetaPlatform Hierarchy](#metaplatform-hierarchy)). For example, the `monodroid` and `monotouch` `MetaPlatforms` each have a `AnyCPU` `ProxiedPlatform` which can be built like this:

    src\carouselView\lib> msbuild /v:m /p:Platform=AnyCPU /p:MetaPlatform=monotouch
    src\carouselView\lib> msbuild /v:m /p:Platform=AnyCPU /p:MetaPlatform=monodroid
    
Though `LeafPlatforms` have no `MetaPlatform` children they may still be composed of various `PartPlatforms` as is described in the following section.
    
### Part Platforms
A `MetaPlatform` may have merged what used to be two separate assemblies (parts) into a single composite assembly. For example, `MetaPlatform` `xamarin.ios` merges what used to be the "portable assembly" ([src\carouselView\lib\portable][5]) and "platform assembly" ([src\carouselView\lib\ios][6]) into a single composite assembly. In addition to simplifying deployment and tree-shaking, merging also opens the possibility that code in what used to be one of the separate assemblies can reference internal members in the code of what used to be the other assembly. Sometimes this is desired. For example, the `RenderWithAttribute` can point directly to the renderer (see  [CarouselViewLibrary.cs][4]). However most of the time this is undesired behavior. So, by default, to raise a compiler error when this happens, under the hood, in parallel to building the composite assembly, the separate portable and platform assemblies are also built. For example, look at the output of a `xamarin.ios` build:
```
src\carouselView\lib> msbuild /v:m /p:MetaPlatform=xamarin.ios
CarouselView -> \bld\bin\debug\carouselView\lib\dotnet\Xamarin.Forms.CarouselView.dll
CarouselView -> \bld\bin\debug\carouselView\lib\xamarin.ios\Xamarin.Forms.CarouselView.dll
```
Three assemblies are created (even though only two are reported if `/v:m` is passed):
- `xamarin.ios\Xamarin.Forms.CarouselView.dll` is the composite assembly (`COMPOSITE` is defined during this build). 
- `dotnet\Xamarin.Forms.CarouselView.dll` is the portable assembly
- and the build of what used to be the platform assembly is not logged but is output to `bld\obj\debug\carouselView\lib\part\xamarin.ios`.

To build just the composite assembly pass `/p:SkipPartBuild=true`:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=xamarin.ios /p:SkipPartBuild=true

To build only the part assemblies pass `/p:IsPartPlatform=true`.

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=xamarin.ios /p:IsPartPlatform=true
    
Search [CarouselView.csproj][2] for `IsPartPlatform` to peek under the hood to see how `PartPlatforms` are declared. Note the declaration of `IsCompositePlatform` which tells the `MetaPlatform` traversal logic to recurse and passing `IsPartPlatform` which effectively hooks the part platform into the build.

## DryRun and Verbosity
Building a `MetaPlatform` results in a traversal of the [MetaPlatform Hierarchy](#metaplatform-hierarchy) which can be visualized by suppressing other logging via `/v:m` and passing `/p:verbosity=high`. To just log the traversal without building use the new `DryRun` `MetaProject` target (see [ext\node\node.targets](ext/node/node.targets)). For example, the following will dump the traversal of `monodroid`:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=monodroid /p:Verbosity=high /t:DryRun

The first frame of output below starts with `RECURSE` which indicates the traversal is starting at an internal (non-`ProxiedPlatform`) node (`BUILD` indicates a `ProxiedPlatform`). In brackets follows the variables that compose the "recursive frame": `[Configuration|Platform|MetaPlatform|Part]` (in this case, `Part` is empty so not shown). Next, `Xamarin.Forms.CarouselView` is the name of the generated assembly and ` -> { anycpu }` shows the children of this node. After that are properties describing the type of the `MetaPlatform` which are used in `.csproj` and `.props` files to configure the project itself (see [ext\xf\xf.pre.props](ext/xf/xf.pre.props)). Finally, `CarouselView [debug|monodroid|monodroid] references:` lists the references (children) and the msbuild command line used to resolve the references. 
```
  RECURSE [debug|monodroid|monodroid]: Xamarin.Forms.CarouselView -> { anycpu }
    BuildTarget -> DryRun
    ProjFile -> F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj
    IsProxyPlatform -> true
    IsMobileLibraryPlatform -> true
    IsMobileLibraryProject -> true
    ProxiedPlatform -> anycpu
    MobilePlatform -> android
    LibraryPlatform -> monodroid
    DefineConstants -> ANDROID;TRACE;DEBUG;
    TargetProject -> monoDroid
    FrameProperties -> MetaPlatform;IsPartPlatform;
  CarouselView [debug|monodroid|monodroid] references:
    ProxiedPlatform: F:\git\xam\cv\src\carouselView\lib\CarouselView.csproj /t:DryRun /+p:_MetaPlatform=;IsPartPlatform=;P
  latform=anycpu;_MetaPlatform=monodroid
  ...
```

## Build Identity
Build identity is established by the `build` alias before compilation begins by executing the following target (or via its alias `BuildInfo`):

    > msbuild %idProj% /v:m

This command will generate the following files in `bld\bin`:

| Name | Description | File |
| --- | --- | --- |
| Number | Next folder in `drp\number` | `BuildInfo.Build.Number.txt` |
| Url | git remote url | `BuildInfo.Enlistment.Revision.txt` |
| Revision | git revision | `BuildInfo.Enlistment.Url.txt` |
| Enlistment Status | git status | `BuildInfo.Enlistment.Status.txt` |

If the repository is dirty (e.g `BuildInfo.Enlistment.Status.txt` is not empty) then a default build number and revision are not assigned and the following warning will be raised:

    Build identity could not be established because enlistment contains modified, new, or untracked files.

Build identity is loaded into the msbuild variables `BuildNumber`, `EnlistementRevision`, and `EnlistmentUrl` and, typically, used to expand C# template files (see below) so build identity can be compiled into assemblies. 

## CSharp Templates
C# templates allow expanding msbuild variables into C# files or asserting that default expansions match an actual expansion at build time; C# Templates have a `.t.cs` extension and during compilation they are not compiled but rather loaded in memory and scanned for variables of the form `$([Name])` which are replaced with the corresponding msbuild variable (e.g. see [`AssemblyVersion.t.cs`][7] and [`BuildInfo.t.cs`](src/BuildInfo.t.cs)). The expanded template is then compared with the matching `.cs` file checked in next to the `.t.cs` file (aka the "default expansion"; see [`AssemblyVersion.cs`][3] and [`BuildInfo.cs`](src/BuildInfo.cs)) and, if found to be different, then either:

1. An warning is issued indicating the default expansion is stale and has been refreshed (a CI build should fail if the build itself causes the enlistment to become dirty and so force the developer to submit the changes for code-review).
2. If `SubstituteExpandedResult=true` metadata is present on the item template (e.g. search for `BuildInfo.t.cs` in [`src\.props`](src/.props)) then the expansion is swapped out for the default expansion; the expansion is saved to `bin\obj\[relative path to src directory]\[tempalte].cs` and included in the compilation while the default expansion is excluded from compilation.

The following build will produce an example of both scenarios:

    src\carouselView\lib >msbuild /p:MetaPlatform=dotnet /t:RefreshTemplateExpansions

```
RefreshTemplateExpansions:
    TemplatePath: \src\BuildInfo.t.cs
    DefaultExpansionPath: \src\BuildInfo.cs
    FreshExpansionPath -> \bld\obj\debug\carouselView\lib\dotnet\BuildInfo.cs
    Variables:
      BuildNumber -> 0
      EnlistmentRevision -> dirty
      EnlistmentUrl -> [null]
    IsExpansionStale: true
      SubstituteExpandedResult: true
    Submsituting: \src\BuildInfo.cs -> \bld\obj\debug\carouselView\lib\dotnet\BuildInfo.cs
RefreshTemplateExpansions:
    TemplatePath: \src\carouselView\lib\Properties\AssemblyVersion.t.cs
    DefaultExpansionPath: \src\carouselView\lib\Properties\AssemblyVersion.cs
    FreshExpansionPath -> \bld\obj\debug\carouselView\lib\dotnet\AssemblyVersion.cs
    Variables:
      BuildVersion -> 2.3.0
```
Try suppling any of the msbuild variables on the command to see what happens when the default expansion becomes dirty. For example:

    src\carouselView\lib >msbuild /p:MetaPlatform=dotnet /t:RefreshTemplateExpansions /p:BuildVersion=2.4.0

## Publishing

### Shim
The [`shim`](ext\shim\shim.proj) is a project which searches for and then builds .csproj files after, among other things, configuring logging. 

### MetaPlatform


and by issuing `/t:dryRun` commands from the shell. For example, here is the [output](doc/dryRun.md) produced by the following command:

    src\carouselView\lib> msbuild /v:m /p:platform=all /t:dryRun

For even more information about project reference resolution pass `/p:verbosity=high` along with any target.

# Future Work

## Hermetic Build Environment
In general, the shell should aspire to do more than simply not depend on Visual Studio and solution files. Ideally, the shell should create a "hermetic build environment" where the build depends on _nothing_ outside of the repo (the one exception, of course, is that the source control client must be installed). This holy grail of build (and test) environments
- guarantees if the build and tests works on a developers machine they'll work on the build server.
- solves the "nuget restore cycle problem" whereby a nuget package restored during build modifies a .csproj after the build has parsed the file.
- vasly simplifies the "getting-started" instructions which encourages community contribution. 

A hermetic build environment is most simply achieved by checking all dependencies into source control (e.g. msbuild.exe, C# compiler, nuget packages emulators, SDKs, and all referenced assemblies of .Net framework). So, just like a Docker container, it is fully self-contained. This is not easily achieved with Git as Git  is not designed to manage binaries out of the box. However Git could fairly easily be augmented to support this (see [git lfs nuget proxy](git-lfs-nuget-proxy)).

## Git Lfs Nuget Proxy

[1]: https://visualstudiogallery.msdn.microsoft.com/b346d9de-8722-4b0e-b50e-9ae9add9fca8
[2]: src/carouselView/lib/CarouselView.csproj
[3]: src/carouselView/lib/Properties/AssemblyVersion.cs
[4]: src/carouselView/lib/Portable/CarouselViewLibrary.cs
[5]: src/carouselView/lib/Portable
[6]: src/carouselView/lib/iOS/
[7]: src/carouselView/lib/Properties/AssemblyVersion.t.cs
