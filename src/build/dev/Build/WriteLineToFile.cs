using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using SysFile = System.IO.File;
using System.Xml.Linq;
using System.Diagnostics;

namespace Xamarin.Forms.Build
{
	public sealed class WriteLineToFile : AbstractTask
	{
		public string Line { get; set; }
		public string File { get; set; }

		protected override void Run()
		{
			using (var sw = new StreamWriter(File, append: true))
				sw.WriteLine(Line);
		}
	}
}
