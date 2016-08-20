using System.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Text.RegularExpressions;

namespace Xamarin.Forms.Build
{
	public sealed class RegexReplace : AbstractTask
	{
		protected override void Run()
		{
			Result = Input;
			foreach (var pattern in Pattern)
			{
				var replacement = pattern.GetMetadata("Replacement");
				Result = Regex.Replace(Result, pattern.ItemSpec, replacement);
			}
		}

		[Required]
		public string Input { get; set; }

		[Required]
		public ITaskItem[] Pattern { get; set; }

		[Output]
		public string Result { get; set; }
	}
}
