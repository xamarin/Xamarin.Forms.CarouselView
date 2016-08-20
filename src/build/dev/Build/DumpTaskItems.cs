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
		public bool ExcludeDefaultMetadata { get; set; }
		public bool ExcludeEmptyValues { get; set; }

		protected override void Run()
		{
			Log.LogMessage(DefaultMessageImportance, Header);

			var indent = "";
			if (Header != null)
				indent = "    ";

			if (Items == null)
				return;

			var sc = StringComparer.InvariantCultureIgnoreCase;

			var items = Items;
			foreach (var item in items.Select(o => new MyTaskItem(o)))
			{
				Log.LogMessage(DefaultMessageImportance, $"{indent}{item}");

				foreach (var metadata in item.Metadata)
				{
					if (ExcludeDefaultMetadata && metadata.IsDefault)
						continue;

					if (ExcludeEmptyValues && metadata.IsNullOrEmpty)
						continue;

					Log.LogMessage(DefaultMessageImportance, $"{indent}{indent}{metadata}");
				}
			}
		}
	}
}
