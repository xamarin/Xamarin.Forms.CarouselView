using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace Xamarin.Forms.Build
{
	public sealed class SwitchFile : AbstractTask
	{
		public ITaskItem[] Items { get; set; }
		public string File { get; set; }

		protected override void Run()
		{
			var xml = new XElement("Project",
				new XElement("Choose",
					new XElement("When")
				)
			);

			xml.Save(File);
		}
	}
}
