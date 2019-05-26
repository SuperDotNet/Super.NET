using System.Collections.Generic;
using System.Reflection;
using Super.Runtime.Environment;
using Super.Runtime.Execution;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Super.Application.Hosting.xUnit
{
	public class Executor : XunitTestFrameworkExecutor
	{
		public Executor(AssemblyName assemblyName,
		                ISourceInformationProvider sourceInformationProvider,
		                IMessageSink diagnosticMessageSink)
			: base(assemblyName, sourceInformationProvider, diagnosticMessageSink) {}

		protected override void RunTestCases(IEnumerable<IXunitTestCase> testCases,
		                                     IMessageSink executionMessageSink,
		                                     ITestFrameworkExecutionOptions executionOptions)
		{
			StorageTypeDefinition.Default.Execute(typeof(Logical<>));
			base.RunTestCases(testCases, executionMessageSink, executionOptions);
		}
	}
}