using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("$(AssemblyName)")]
[assembly: AssemblyDescription("$(NuspecDescription)")]
[assembly: AssemblyConfiguration("$(Configuration)")]

[assembly: AssemblyCompany("$(NuspecOwners)")]
[assembly: AssemblyProduct("$(NuspecId)")]
[assembly: AssemblyCopyright("$(NuspecCopyright)")]
[assembly: AssemblyTrademark("")]

// type conflicts with the imported type; happens when IVT is used
#pragma warning disable 0436
[assembly: AssemblyVersion(BuildVersion.MajorVersion + "." + BuildVersion.MinorVersion + "." + BuildVersion.Number)]
[assembly: NugetPackage("$(NuspecId)", "$(NuspecVersion)")]

[assembly: NugetDependency("%(NugetDependency.Name)", "%(Version)")]
[assembly: NugetTargetFramework("$(NugetTargetFrameworkMoniker)", Version = "$(NugetTargetFrameworkVersion)")]

[assembly: EnlistmentUrl("$(EnlistmentUrl)")]
[assembly: EnlistmentBranch("$(EnslitmentBranch)")]
[assembly: EnlistmentRevision("$(EnlistmentRevision)")]

#if NUGET_BUILD_ASSEMBLY
[assembly: NugetBuildAssembly(Directory = "$(NugetBuildAssemblySubDirName)")]
#endif
#pragma warning restore 0436

[assembly: ComVisible(false)]
