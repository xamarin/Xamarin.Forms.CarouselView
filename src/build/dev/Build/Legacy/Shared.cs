using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Xml.Linq;

namespace Xamarin.Forms.Build
{
	public static class Extensions
	{
		public static string NormalizeSlashes(this Uri path)
		{
			return path.ToString().NormalizeSlashes();
		}
		public static string NormalizeSlashes(this string path)
		{
			path = path.Replace('/', Path.DirectorySeparatorChar);
			path = path.Replace('\\', Path.DirectorySeparatorChar);
			return path;
		}
		public static string NormalizeDir(this string dir)
		{
			dir = dir.NormalizeSlashes();
			if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString()))
				dir = dir + Path.DirectorySeparatorChar;
			return dir;
		}
	}

	public sealed class TaskException : Exception
	{
		public TaskException(string message) : base(message) { }
	}

	public abstract class AbstractTask : Task
	{
		protected abstract void Run();

		public override bool Execute()
		{
			try
			{
				Run();
			}
			catch (TaskException e)
			{
				Log.LogError(e.Message);
			}
			catch (Exception e)
			{
				Log.LogError(e.ToString());
			}

			return !Log.HasLoggedErrors;
		}
	}

	public sealed class TaskMetadata
	{
		public static string[] s_defaultMetadataNames = new[] {
			"FullPath",
			"RootDir",
			"Filename",
			"Extension",
			"RelativeDir",
			"RecursiveDir",
			"Identity",
			"ModifiedTime",
			"CreatedTime",
			"AccessedTime",
			"Directory",
			"DefiningProjectFullPath",
			"DefiningProjectDirectory",
			"DefiningProjectName",
			"DefiningProjectExtension",
			"OriginalItemSpec",
			"MSBuildSourceProjectFile",
			"MSBuildSourceTargetName",
		};

		MyTaskItem m_taskItem;
		string m_name;

		public TaskMetadata(MyTaskItem taskItem, string name)
		{
			m_taskItem = taskItem;
			m_name = name;
		}

		public MyTaskItem TaskItem => m_taskItem;
		public string Name => m_name;
		public string Value => m_taskItem.Get(Name);
		public bool IsNullOrEmpty => string.IsNullOrEmpty(Value);
		public bool IsDefault =>  s_defaultMetadataNames.Contains(Name, StringComparer.InvariantCultureIgnoreCase);

		public override string ToString() => $"{Name}: {Value}";
	}
	public sealed class MyTaskItem
	{
		ITaskItem m_taskItem;

		public MyTaskItem(ITaskItem taskItem)
		{
			m_taskItem = taskItem;
		}

		public ITaskItem Item => m_taskItem;
		public string ItemSpec => m_taskItem.ItemSpec;

		public string Get(string name) => m_taskItem.GetMetadata(name);
		public string Identity => Get(nameof(Identity));
		public IEnumerable<TaskMetadata> Metadata => 
			from string o in Item.MetadataNames
			select new TaskMetadata(this, o);

		public override string ToString() => Identity;
	}

	public class CammelCase
	{
		public static implicit operator XName(CammelCase cammelCase) => cammelCase.ToString();
		public static implicit operator string(CammelCase cammelCase) => cammelCase.ToString();
		public static implicit operator CammelCase(string identifier) => new CammelCase(identifier);

		private string m_identifier;

		public CammelCase(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
				identifier = "";
			m_identifier = char.ToLower(identifier[0]).ToString();

			if (identifier.Length > 1)
				m_identifier += identifier.Substring(1);
		}

		public override string ToString() => m_identifier;
	}

	public class ProjectReference
	{
		MyTaskItem m_taskItem;

		public ProjectReference(ITaskItem taskItem)
			: this(new MyTaskItem(taskItem))
		{
		}
		public ProjectReference(MyTaskItem taskItem)
		{
			m_taskItem = taskItem;
		}

		public string Get(string name) => m_taskItem.Get(name);

		public Guid Project => new Guid(Get(nameof(Project)));
		public string Name => Get(nameof(Name));
		public bool Private => bool.Parse(Get(nameof(Private)));
		public string FullPath => Get(nameof(FullPath));
		public string RootDir => Get(nameof(RootDir));
		public string FileName => Get(nameof(FileName));
		public string Extension => Get(nameof(Extension));
		public string RelativeDir => Get(nameof(RelativeDir));
		public string Directory => Get(nameof(Directory));
		public DateTime ModifiedTime => DateTime.Parse(Get(nameof(ModifiedTime)));
		public DateTime CreatedTime => DateTime.Parse(Get(nameof(CreatedTime)));
		public DateTime AccessedTime => DateTime.Parse(Get(nameof(AccessedTime)));
		public string DefiningProjectFullPath => Get(nameof(DefiningProjectFullPath));
		public string DefiningProjectDirectory => Get(nameof(DefiningProjectDirectory));
		public string DefiningProjectName => Get(nameof(DefiningProjectName));
		public string DefiningProjectExtension => Get(nameof(DefiningProjectExtension));
	}

	public class ResolvedProjectReference : ProjectReference
	{
		public ResolvedProjectReference(ITaskItem taskItem)
			: this(new MyTaskItem(taskItem))
		{
		}
		public ResolvedProjectReference(MyTaskItem taskItem)
			: base(taskItem)
		{
		}

		public bool BuildReference => bool.Parse(Get(nameof(BuildReference)));
		public bool ReferenceOutputAssembly => bool.Parse(Get(nameof(ReferenceOutputAssembly)));
		public string MSBuildSourceProjectFile => Get(nameof(MSBuildSourceProjectFile));
		public string MSBuildSourceTargetName => Get(nameof(MSBuildSourceTargetName));
		public string OriginalProjectReferenceItemSpec => Get(nameof(OriginalProjectReferenceItemSpec));
	}
}
