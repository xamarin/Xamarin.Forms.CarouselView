# Quick Start
Xamarin.Forms.Carousel repo contains an alpha Xamarin Forms build environment. Similar to other .NET foundation repos (e.g. coreclr) everything starts in a shell. From there you'll need to build (or at least restore nuget packages) before opening the solution. The next generation build environment is being developed in the xfproj branch. For now, the following commands should get you off the ground:

## Opening Solution

1. open shell using `\env.lnk`
2. type `restore` to restore nuget files
3. in `src\build` type `build` (to build custom msbuild tasks)
3. open `\src\Xamarin.Forms.CarouselView.sln`

## Building like CI does

0. configure machine so that a vanilla Xamarin.Forms project builds
1. open shell using `\env.lnk`
2. type `build` to build both debug and release package
3. nuget package will be at `\bin\bld\release\carouselView\nuget\`
4. the build will be archived at `\drp\number\10001\`

# Xamarin.Forms Build System
The Xamarin.Forms build system addresses challenges encountered while developing CarouselView with the immediate goal of simplifying design, build, test, and packaging of Xamarin.Forms libraries. 

The Xamarin.Forms build systems aims to do at design time what Xamarin.Forms has already done at runtime. At runtime, Xamarin.Forms abstracts the different types of mobile platforms by allowing virtual controls to be added to a virtual device which Xamarin.Forms then renders for each specific platform. At design time, the Xamarin.Forms build system aims to abstract the different types of mobile projects by merging them into a single project from which the build system generates multipule assemblies.

Providing CI "out-of-the-box" is also a goal of Xamarin.Forms build. In practice, this means having the ability to take a machine with a freshly installed OS and in a single command (possibly powershell now that it's cross platform), install Visual Stuido, install Xamarin Studio, install 3ed party tools (git, nuget, etc), download the source, clean, restore, build, package, publish, deploy, and test on all platforms. This works focuses on the clean, restore, build, package, and publish steps. 

The documentation is dividied into [Highlighs](#highlighs) of the build system specific to Xamarin.Forms and [General Build System](#general-build-system) which describes the numerous more general build enhancements on top of which the new Xamarin.Forms build system is built.

## Highlights
Consuming and producing Xamarin.Forms libraries is simplified chiefly by [reducing the number of projects](#project-reduction) consumed and produced by both mobile apps and libraries, by the introduction of a [mobile meta-platform tree](#mobile-metaplatform-tree) which integrates with existing Visual Studio [solution](#mobile-solution) and [project](#mobile-project) tooling, and finally, "de-duplicating" the msbuild files so build setting like `TreatWarningsAsErrors` appear only once and so can be centrally administered.

### Project Reduction
The number of binaries a Xamarin.Forms app needs to reference to use a Xamarin.Forms library is reduced from 3 (portable, platform, and shim [to support the linker]) to 1. This is achieved by compiling the portable and shim logic into the platform library. This allows a `RenderWithAttribute` applied to the Xamarin.Forms element to directly reference the platform renderer ([see here][2]). This obviates the need for the shim library and dodges a large class of potential linker issues. A compiler error is still generated during library construction if the platform logic references internal portable logic (note that until VSIP integration happens, Intellisense will not complain about such references). Under the hood, this is achieved by kicking off additional compilations of the project. The code can check for the `COMPOSITE` compilation symbol to know what type of compilation is occurring. 

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
nuke - use git to clean out directory and sub directories
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
Builds are archived so they may be distributed via file servers. Among other things, having an archive of builds simplifies bisecting a regression and debugging intermittent build failures. For a build to be archived it must have been assigned an identity and that identity should be burned into each assembly published. Build identities are assigned by the shim (see [shim](#shim)) to builds of clean enlistments kicked off from the root.

The following three identities are assigned to a build:

| Assigned By | Identity | Example |
| --- | --- | --- | --- |
| File system | Number | Next folder in `drp\number` (see [`BuildInfo.cs`](src/BuildInfo.cs)) |
| Source Control | Revision, Branch, Url | Git hash, branch, url (see [`BuildInfo.cs`](src/BuildInfo.cs)) |
| Person | Version | `BuildVersion` declared in [`src\.props`](src/.props) (see [`AssemblyVersion.cs`][3]) |

After build completion, identifiable builds will have their `bin\bld` directory copied to `drp\number\$(BuildNumber)` and a simlink to that directory is created at `drp\revision\$(EnlistmentRevision)` and created (or overridden) at `drp\version\$(BuildVersion)`. 
````
▌ drp
├──▌ number - archived builds by build number
├──▌ revision - archived builds by source control revision  (symlinks into drp\number)
└──▌ version - archived builds by version (symlinks into drp\number)
````

## Projects
Project files have been modified to enable build features that simplify maintaining a CI infrastructure at the expense of tooling. The main tooling break are design time features of Visual Studio which modify project files because, without VSIP integration, Visual Studio in unaware of the new conventions. For example, adding a new file Android specific file via Visual Studio to [`CarouselView.csproj`][2] will require moving the `<Compile Include="NewAndroidFile.cs">` element to live under `<ItemGroup Condition=" '$(MobilePlatform)' == '$(AndroidMobilePlatformId)'" >`. 

Manual edits of project files are more easily made after installing [`EditProj`][1]. For more extensive edits, unload all projects under `src` and open project files from the shared project [`.repo`](.repo.shproj). 

The `.repo` project includes all msbuild files. This allows for global search and replace of msbuild symbols. The shell alias `ts` touches the solution file which has the effect of reloading changes made to msbuild files which are included by project files which are otherwise cashed once at startup and never refreshed.

### General Project Template
Project files all conform to the following general template sections of which are described in subsequent sections.

```xml
<Project>
  <PropertyGroup>
    <!--Properties that identify the type of the project-->
  </PropertyGroup>
  
  <!--Load properties that are a function of the type of the project and platform-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildProjectDirectory)\, .pre.props))\.pre.props" />

  <!--Load common properties for the given type of the project and platform-->
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .props))\.props" />

  <PropertyGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--Set properties specific to this project and\or platform; the goal is to have as few such properties as possible-->
  </PropertyGroup>
  <ItemGroup Condition="'$(SomeTypeProperty)'=='SomeTypeId'">
    <!--Project Files-->
    <!--Project References-->
  </ItemGroup>
  
  <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory), .targets))\.targets" />
</Project>
```

### Common Properties (.props)
Typically, solutions with multiple projects suffer from duplication of project settings. For example, to enable warnings as errors typically requires modifying each project. To prevent duplication and allow settings to be centrally administered common project settings are extracted to `.props` files in parent directories. For example, `WarningLevel` and `TreatWarningsAsErrors` have been extracted to [`src\.props`](src/.props) and so are included by all projects in any sub-directory of `src`. 

Those `.props` files "closest" to the project override those files further away. For example, the [`.props`](.props) file in root directory is included before any other simultaneously making its definitions available to those files and allowing them to override those settings. For example, the `.props` files processed when loading [`CarouselView.csproj`][2] are, [`\.props`](.props), then [`src\.props`](src/.props), then [`src\carouselView\.props`](src/carouselview/.props), then finally the project itself. Note that they are included in the reverse order but, because the first thing they each do is import their parent `.props` file they are logically processed in the reverse of the include order. Don't think too hard about it; It works as you'd expect.

Some settings gathered into `.props` files are common to all projects (e.g. `WarningLevel` and `TreatWarningsAsErrors`) however most are common to a specific _type_ of platform or project. For example, [`src/.props'](src/.props) (where most of settings extracted from various types of projects end up) contains the following section describing debug settings:
````
  <!--debug-->
  <PropertyGroup Condition="'$(Configuration)'=='debug'">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <DefineConstants>$(DefineConstants)TRACE;DEBUG;</DefineConstants>
  </PropertyGroup>
````
In general, such sections take the following form:
````
  <!--common setting for type-->
  <PropertyGroup Condition="'$(Type)'=='TypeId'">
    <TypeSetting>setting</TypeSetting>
  </PropertyGroup>
````
Before a `.props` file can determine the type of project loaded, and so what settings are appropriate, properties describing the type of the project must be loaded. How type information about a project is populated is the subject of the next section.

### Project and Platform Types (.pre.props)
The first property defined by all projects is `<MetaProject>` which in conjunction with the `Configuration`, `Platform`, and `MetaPlatform` global properties (passed via the command line or through the `msbuild` task), identifies the _type_ of the project. With the type established, a hierarchy of `.pre.props` files (which are loaded before `.props` files) are able to populate properties which fully describe all aspects of the type of the project being loaded and which are documented below.

#### MetaProject
A `MetaProject` is an amalgam of projects. For example, all the Xamarin.Forms library projects (e.g. Android, iOS, and Windows) combine to form the `xf.lib` `MetaProject`, Xamarin.Forms app projects form `xf.app`, and Calabash Android and iOS UI automation projects form `xf.aut`. One of the boolean properties `IsMobileLibraryProject`, `IsMobileAppProject`, or `IsMobileTestProject` is set to `true` depending on the type of meta-project being loaded.

#### MetaPlatform
`MetaPlatform` is the heart of the type system; The `MetaPlatform` abstraction allows grafting a new platform lexicon over of the existing desktop lexicon. For example, in the case of Xamarin.Forms, `MetaPlatform` makes it possible to talk about `mobile`, `android`, or `win.arm` platforms instead of `AnyCPU`, `x32`, or `x64` platforms. 

Each `MetaProject` supports a set of `MetaPlatforms`. For example, here are the relationships for Xamarin.Forms:

| MetaPlatform | MetaProject |
| --- | --- |
| xf.aut | android.aut, ios.aut |
| xf.lib | dotnet, monodroid, monotouch, xamarin.ios, win, uap, wpa |
| xf.app | monodroid.app, monotouch.phone, monotouch.sim, xamarin.ios.sim, xamarin.ios.phone, win.32, win.64, win.arm, uap.32, wpa.32 |

A `MetaPlatform` will have a `MetaPlatformType` of either `group`, `meta`, or `leaf`. Depending on the `MetaPlatformType`, either `IsGroupPlatform`, `IsMetaPlatform` or `IsLeafPlatform` will be set to true.
- A `group` `MetaPlatform` is a collection of one or more `group` or `meta` `MetaPlatforms`. Groups can be named (e.g. `Mobile` = { `portable`, `android`, `ios`, `windows` }) or specified ad-hoc on the command line (e.g. `/p:MetaPlatform=android;windows`). The children of the group are stored in `ChildMetaPlatforms`. 
- A `meta` `MetaPlatform` is a platform in new lexicon which is being grafted over a desktop platform (e.g. `monodroid` over `AnyCpu` or `monotouch.app.sim` over `IPhoneSimulator`). It has one `leaf` `MetaPlatform` child which is stored in `LeafPlatform`.
- A `leaf` `MetaPlatform` is a desktop platform (e.g. `AnyCPU`) augmented with a `MetaPlatform` (e.g. `android`) and `MetaProject` (e.g. `android.lib`) and other properties describing the project type (e.g. `MobilePlatform`==`Android`). These agumented properties are use in `.csproj` and `.props` files (e.g.  [`CarouselView.csproj`][2] and [`src/.props`](src/.props)) to set the properties (e.g. `AndroidSupportedAbis`) of one of the projects composing the unified project (e.g. Xamarin.Android). Once the properties of the composed project are set its msbuild targets are invoked to finally preform the build.

#### MetaPlatform Hierarchy
Here is a summary of the relationships between `MetaProjects`, `MetaPlatforms`, and `MetaPlatformTypes` for Xamarin.Forms. These relationships are declared in [`ext\xf\xf.pre.props`](ext/xf/xf.pre.props).

![Platform Image](doc/Platforms.gif)

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

## Restoring, Building, And Cleaning a specific MetaPlatform 
Each `MetaPlatform` in a `MetaProject` can be restored, built, and cleaned separately by passing `/p:MetaPlatform=[MetaPlatform]` to `msbuild` as explained below. Note, before running any of the following examples on a `MetaProject` that project will need to be restored using an approach listed in [Restoring](#restoring).

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
    
The ur group `all` will build all `MetaPlatforms` in a `MetaProject`. For example, the following will build (and package) all binaries that compose the CarouselView nuget package:

    src\carouselView\lib> msbuild /v:m /p:MetaPlatform=all
    
If no `MetaPlatform` is passed, then `all` `MetaPlatform` is assigned as a default. 

    
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
