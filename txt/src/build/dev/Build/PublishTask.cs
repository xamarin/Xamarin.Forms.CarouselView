using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace Xamarin.Forms.Build
{
	public abstract class PublishTask : AbstractTask
	{
		public static string Identity = "identity";

		public string BaseUri { get; set; }

		[Required]
		public string Uri { get; set; }
	}

	public sealed class PublishItemTask : PublishTask
	{
		protected override void Run()
		{
		}

		[Required]
		public ITaskItem[] Item { get; set; }
	}

	public sealed class PublishValue : PublishTask
	{
		void Publish(Uri property)
		{
			Log.LogMessage($"Property: {property}");
		}

		protected override void Run()
		{
			// build uri
			var baseUri = new Uri(BaseUri);
			var relUri = new Uri(Uri, UriKind.RelativeOrAbsolute);
			var uri = new Uri(baseUri, relUri);

			// uri -> path
			var path = uri.AbsolutePath;
			var dir = Path.GetDirectoryName(path);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			// publish
			Log.LogMessage(MessageImportance.Normal, $"{relUri} = {Value}");
			File.WriteAllText(path, Value);
		}

		public string Value { get; set; }
	}
}
