using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using NUnit.Framework;

namespace Xamarin.Forms.Build
{
	public class Msbuild
	{
		LoggerVerbosity m_verbosity;
		string m_path;

		public Msbuild(string path, LoggerVerbosity verbosity = LoggerVerbosity.Detailed)
		{
			m_verbosity = verbosity;
			m_path = path;
		}

		ILogger CreateLogger(StringBuilder builder)
		{
			var writer = new StringWriter(builder);
			WriteHandler handler = (x) => writer.WriteLine(x);

			var consoleLogger = new ConsoleLogger(
				m_verbosity,
				write: handler,
				colorSet: null,
				colorReset: null
			);

			return consoleLogger;
		}

		public string Build(string target)
		{
			var builder = new StringBuilder();

			var loggers = new List<ILogger>();

			// capture log output to string for test assertions
			loggers.Add(CreateLogger(builder));

			// capture log output for display by nunit test results
			loggers.Add(new ConsoleLogger(m_verbosity));

			var projectCollection = new ProjectCollection();
			projectCollection.RegisterLoggers(loggers);

			var project = projectCollection.LoadProject(m_path);

			try
			{
				project.Build(target);
			}
			finally
			{
				projectCollection.UnregisterAllLoggers();
			}

			return builder.ToString();
		}
	}

	[TestFixture]
	public class TaskTest
	{
		static string ProjectFile = "test.targets";
		static string Success = "Build succeeded.";
		static string Failed = "Build FAILED.";

		[Test]
		public void Empty()
		{
			string consoleOutput = new Msbuild(ProjectFile).Build(nameof(Empty));
			StringAssert.Contains(Success, consoleOutput);
		}

		[Test]
		public void PublishValueTrivial()
		{
			string consoleOutput = new Msbuild(ProjectFile).Build(nameof(PublishValueTrivial));
			StringAssert.Contains(Success, consoleOutput);
		}

		[Test]
		public void PublishValueNoValue()
		{
			string consoleOutput = new Msbuild(ProjectFile).Build(nameof(PublishValueNoValue));
			StringAssert.Contains(Failed, consoleOutput);
		}
	}
}
