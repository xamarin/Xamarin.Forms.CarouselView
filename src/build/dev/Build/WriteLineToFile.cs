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
	public sealed class WriteTextToFile : AbstractTask
	{
		public string File { get; set; }
		public string Text { get; set; }
		public bool Overwrite { get; set; }

		protected override void Run()
		{
			var append = !Overwrite;
			using (var sw = new StreamWriter(File, append: append))
				sw.Write(Text);
		}
	}
}
