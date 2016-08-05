# Build Environment
Xamarin.Forms.Carousel repo contains an alpha Xamarin Forms build enviroment. Similar to other .NET foundation repos (e.g. coreclr) everything starts in a shell. From there you'll need to build (or at least restore nuget packages) before opening the solution. The next generation build environment is being developed in the xfproj branch. For now the following commands should get you off the ground:

# Opening Solution

1. open cmd.exe using [this link][1]
2. ensure `git.exe` is available from the launched shell
3. type `sln` 


# Building for Release

1. close all instance of Visual Studio
2. open `cmd.exe` using [this link][1]
3. type `killusual`
4. type `git status` to ensure repro has no open or untracked files
5. type `nuke` to use `git clean` to restore repo to a clean state
6. type `src`
7. type `rbuild` to build a release package (just `build` for debug)
8. Your nuget package will be at `bin\bld\release\carouselView\nuget\Package\Xamarin.Forms.CarouselView.2.3.0-pre2.nupkg`

[1]: https://github.com/xamarin/Xamarin.Forms.Carousel/blob/master/txt/env/env.lnk
