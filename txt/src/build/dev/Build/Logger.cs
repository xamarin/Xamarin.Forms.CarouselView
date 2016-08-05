using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Net;
using Microsoft.Build.Logging;
using System.Diagnostics;

namespace Xamarin.Forms.Build
{
	public class XamarinLogger : ILogger, IEventSource
	{
		FileLogger m_fileLogger;

		public event BuildMessageEventHandler MessageRaised;
		public event BuildErrorEventHandler ErrorRaised;
		public event BuildWarningEventHandler WarningRaised;
		public event BuildStartedEventHandler BuildStarted;
		public event BuildFinishedEventHandler BuildFinished;
		public event ProjectStartedEventHandler ProjectStarted;
		public event ProjectFinishedEventHandler ProjectFinished;
		public event TargetStartedEventHandler TargetStarted;
		public event TargetFinishedEventHandler TargetFinished;
		public event TaskStartedEventHandler TaskStarted;
		public event TaskFinishedEventHandler TaskFinished;
		public event CustomBuildEventHandler CustomEventRaised;
		public event BuildStatusEventHandler StatusEventRaised;
		public event AnyEventHandler AnyEventRaised;

		public XamarinLogger()
		{
			m_fileLogger = new FileLogger();
		}

		public void Initialize(IEventSource eventSource)
		{
			eventSource.MessageRaised += delegate(object sender, BuildMessageEventArgs e)
			{
				MessageRaised(sender, e);
			};
			eventSource.ErrorRaised += ErrorRaised;
			eventSource.WarningRaised += WarningRaised;

			eventSource.BuildStarted += BuildStarted;
			eventSource.BuildFinished += BuildFinished;

			eventSource.ProjectStarted += ProjectStarted;
			eventSource.ProjectFinished += ProjectFinished;

			eventSource.TargetStarted += TargetStarted;
			eventSource.TargetFinished += TargetFinished;

			eventSource.TaskStarted += TaskStarted;
			eventSource.TaskFinished += TaskFinished;

			eventSource.CustomEventRaised += CustomEventRaised;
			eventSource.StatusEventRaised += StatusEventRaised;

			eventSource.AnyEventRaised += AnyEventRaised;

			m_fileLogger.Initialize(this);
		}
		public string Parameters
		{
			get
			{
				return m_fileLogger.Parameters;
			}

			set
			{
				m_fileLogger.Parameters = value;
			}
		}
		public LoggerVerbosity Verbosity
		{
			get
			{
				return m_fileLogger.Verbosity;
			}

			set
			{
				m_fileLogger.Verbosity = value;
			}
		}
		public void Shutdown()
		{
			m_fileLogger.Shutdown();
		}
	}
}
