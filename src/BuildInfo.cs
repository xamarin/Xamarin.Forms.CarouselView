using System;
using System.Reflection;

[assembly: AssemblyVersion(BuildInfo.MajorVersion + "." + BuildInfo.MinorVersion + "." + BuildInfo.Number)]

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
    internal const string Number = "00";
    internal const string EnlistmentRevision = "";
    internal const string EnlistmentUrl = "";
    internal const string EnslitmentBranch = "";

    internal const string MajorVersion = "0";
    internal const string MinorVersion = "0";
}
