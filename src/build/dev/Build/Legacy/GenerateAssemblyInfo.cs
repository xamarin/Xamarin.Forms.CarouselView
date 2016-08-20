using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using IOPath = System.IO.Path;
using System.Net;
using Microsoft.Build.Logging;
using System.Diagnostics;

namespace Xamarin.Forms.Build
{
	public sealed class GenAssemblyInfoFile : AbstractTask
	{
		protected override void Run()
		{
			var nl = Environment.NewLine;
			var assemblyVersion = BuildVersion;
			if (BuildNumber != null)
				assemblyVersion += "." + BuildNumber;

			// generate file
			var text = "";
			text += $"using System.Reflection;" + nl;
			text += nl;
			text += $"[assembly: AssemblyVersion(\"{assemblyVersion}\")]" + nl;
			text += $"//[assembly: AssemblySource(\"{SourceControlUrl}\", \"{SourceControlBranch}\", \"{SourceControlId}\")]" + nl;
			text += $"" + nl;

			// create directory
			var dir = IOPath.GetDirectoryName(Path);
			if (!(Directory.Exists(dir)))
				Directory.CreateDirectory(dir);

			// write file
			var fileName = IOPath.GetFileName(Path);
			Log.LogMessage($"{fileName} -> {Path}");
			File.WriteAllText(Path, text);
		}

		[Required]
		public string Path { get; set; }
		[Required]
		public string BuildVersion { get; set; }

		public string BuildNumber { get; set; }
		public string SourceControlUrl { get; set; }
		public string SourceControlId { get; set; }
		public string SourceControlBranch { get; set; }
	}
}
