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
	public sealed class DumpTaskItems : AbstractTask
	{
		static MessageImportance DefaultMessageImportance = MessageImportance.High;
		
		public string Header { get; set; }
		public ITaskItem[] Items { get; set; }

		protected override void Run()
		{
			Log.LogMessage(DefaultMessageImportance, Header);

			var buffer = "";
			if (Header != null)
				buffer = "    ";

			var items = Items.Select(o => new TaskItem(o));
			foreach (var item in items)
				Log.LogMessage(DefaultMessageImportance, item.ToString(buffer));
		}
	}
}
