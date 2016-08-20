using System.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Text.RegularExpressions;

namespace Xamarin.Forms.Build
{
	public sealed class RegexMatches : AbstractTask
	{
		protected override void Run() =>
			Matches = Regex.Matches(Input, Pattern)
				.Cast<Match>()
				.Select(o => new TaskItem(o.Value))
				.ToArray();

		[Required]
		public string Input { get; set; }

		[Required]
		public string Pattern { get; set; }

		[Output]
		public ITaskItem[] Matches { get; set; }
	}
}
