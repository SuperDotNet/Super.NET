﻿using System.Reflection;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Super.Application.Hosting.xUnit
{
	[UsedImplicitly]
	public sealed class TestFramework : XunitTestFramework
	{
		public TestFramework(IMessageSink messageSink) : base(messageSink) {}

		protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
			=> new Executor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
	}
}