using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Net;

namespace Xamarin.Forms.Build
{
	public sealed class WebCopy : AbstractTask
	{
		protected override void Run()
		{
			var webClient = new WebClient();

			var dirUrl = new Uri(DestinationFolder.NormalizeSlashes());
			var urls = SourceUrls.Split(';').Select(o => new Uri(o));
			foreach (var url in urls)
			{
				var fileName = url.Segments.Last();
				var path = new Uri(dirUrl, fileName).LocalPath;

				Log.LogMessage($"{url} -> {path}");

				var dir = Path.GetDirectoryName(path);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);

				webClient.DownloadFile(
					address: url.ToString(), 
					fileName: path
				);
			}
		}

		[Required]
		public string SourceUrls { get; set; }

		[Required]
		public string DestinationFolder { get; set; }
	}
}
