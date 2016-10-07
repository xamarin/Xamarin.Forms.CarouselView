using System;
using System.Reflection;

[assembly: AssemblyVersion(BuildInfo.MajorVersion + "." + BuildInfo.MinorVersion + "." + BuildInfo.Number)]

[assembly: NugetReference("%(NugetReference.Package)", "%(Version)", "$(LibraryPlatform)", "%(Identity)")]

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
internal class NugetReferenceAttribute : Attribute {

    internal NugetReferenceAttribute(string package, string version, string targetFramework, string assembly) {
        TargetFramework = targetFramework;
        Package = package;
        Version = version;
        Assembly = assembly;
    }

    public string Assembly { get; private set; }
    public string Package { get; private set; }
    public string Version { get; private set; }
    public string TargetFramework { get; private set; }
}

internal class BuildInfo {
    internal const string Number = "$(BuildNumber)";
    internal const string EnlistmentRevision = "$(EnlistmentRevision)";
    internal const string EnlistmentUrl = "$(EnlistmentUrl)";
    internal const string EnslitmentBranch = "$(EnlistmentBranch)";

    internal const string MajorVersion = "$(BuildMajorVersion)";
    internal const string MinorVersion = "$(BuildMinorVersion)";
}
