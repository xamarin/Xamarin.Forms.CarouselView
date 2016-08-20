# Build Environment
Xamarin.Forms.Carousel repo contains an alpha Xamarin Forms build enviroment. Similar to other .NET foundation repos (e.g. coreclr) everything starts in a shell. From there you'll need to build (or at least restore nuget packages) before opening the solution. The next generation build environment is being developed in the xfproj branch. For now the following commands should get you off the ground:

## Opening Solution

1. open cmd.exe using [this link][1]
2. ensure `git.exe` is available from the launched shell
3. type `sln` 


## Building for Release

1. close all instance of Visual Studio
2. open `cmd.exe` using [this link][1]
3. type `killusual`
4. type `git status` to ensure repro has no open or untracked files
5. type `nuke` to use `git clean` to restore repo to a clean state
6. type `src`
7. type `rbuild` to build a release package (just `build` for debug)
8. Your nuget package will be at `bin\bld\release\carouselView\nuget\Package\Xamarin.Forms.CarouselView.2.3.0-pre2.nupkg`

# History

The Xamarin.Forms library build environment addresses challenges encountered while developing CarouselView with the goal of simplifying the creation of Xamarin.Forms libraries in general. 

The number of binaries an app needs to reference to use a library is reduced from 3 (portable, platform, and shim [to support the linker]) to 1. This is achived by compiling the portable (and shim) logic into the platform library. This allows a `RenderWithAttribute` applied to the portable renderer to directly reference the platform renderer ([see here][2]). This obviates the need for the shim library and dodges a large class of potential linker issues. A compiler error is still generated if the platform logic references internal portable logic. Under the hood, this is achived by kicking off additional compilations of the project. The code can check for the the `COMPOSITE` compilation symbol to know what type of compilation is occuring.

The number of C# projects required for building a library is been reduced from 7 (e.g. a project for Portable, Android, iOS [Classic & Unified], and Windows [tablet, phone, uap] to 1. This is achived (with some effort!) by "merging" the 7 project files into a single project file with each merged project file becoming its own platform. So, for example, the Android CarouselView library can be built like this:

    src\carouselView\lib> msbuild myLibrary.csproj /p:platform=monodroid

To build the iOS version substitute `monodroid` with `monotouch`. To build all platforms at once use the platform `mobile`. To peek under the hood, pass `/t:dryRun` to dump the tree traversal used to build these "meta-platforms".

Tooling support for the unified project is as best as can be done without creating a VSIP project system. Intellisense, debugging, and reflecting over some the project settings can be made to work (e.g. when Properties is opened the compilation symbols will be correct) however edits to the unified project via the UI are generally not supported (e.g. adding a new file for a specific platform via the UI will require editing the project file by hand and moving the Compile reference under the [ItemGroup][3] which the correct Condition for the platform in which it's expected to be compiled). Again, to support UI operations a Xamarin.Forms VSIP plugin will need to be developed.

The project contains a folder for each platform the contents of which will only be compiled (and have correct intellisense) when the corrisponding platform is selected. Debugging is gently hacked in; Ideally, to debug the android application select the CarouselView.App.Android csproj.

__ 


[1]: https://github.com/xamarin/Xamarin.Forms.Carousel/blob/master/env/env.lnk
[2]: https://github.com/xamarin/Xamarin.Forms.CarouselView/blob/xfproj/src/carouselView/lib/Portable/CarouselView.cs#L10
[3]: https://github.com/xamarin/Xamarin.Forms.CarouselView/blob/xfproj/src/carouselView/lib/CarouselView.csproj#L122
