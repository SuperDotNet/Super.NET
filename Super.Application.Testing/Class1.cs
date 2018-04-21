using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

// ReSharper disable All

namespace Super.Application.Testing
{
	[UsedImplicitly]
	public sealed class TestFramework : XunitTestFramework
	{
		public TestFramework(IMessageSink messageSink) : base(messageSink) {}

		protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
			=> new Executor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
	}

	public class Executor : XunitTestFrameworkExecutor
	{
		public Executor(AssemblyName assemblyName,
		                ISourceInformationProvider sourceInformationProvider,
		                IMessageSink diagnosticMessageSink)
			: base(assemblyName, sourceInformationProvider, diagnosticMessageSink) {}

		protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink,
		                                           ITestFrameworkExecutionOptions executionOptions)
		{
			using (var assemblyRunner = new AssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink,
			                                               executionOptions))
			{
				await assemblyRunner.RunAsync();
			}
		}
	}

	public class AssemblyRunner : XunitTestAssemblyRunner
	{
		public AssemblyRunner(ITestAssembly testAssembly,
		                      IEnumerable<IXunitTestCase> testCases,
		                      IMessageSink diagnosticMessageSink,
		                      IMessageSink executionMessageSink,
		                      ITestFrameworkExecutionOptions executionOptions)
			: base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions) {}

		protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
		                                                           ITestCollection testCollection,
		                                                           IEnumerable<IXunitTestCase> testCases,
		                                                           CancellationTokenSource cancellationTokenSource)
			=> new TestCollectionRunner(testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer,
			                            new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
	}

	public class TestCollectionRunner : XunitTestCollectionRunner
	{
		readonly IMessageSink diagnosticMessageSink;

		public TestCollectionRunner(ITestCollection testCollection,
		                            IEnumerable<IXunitTestCase> testCases,
		                            IMessageSink diagnosticMessageSink,
		                            IMessageBus messageBus,
		                            ITestCaseOrderer testCaseOrderer,
		                            ExceptionAggregator aggregator,
		                            CancellationTokenSource cancellationTokenSource)
			: base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator,
			       cancellationTokenSource) => this.diagnosticMessageSink = diagnosticMessageSink;

		protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class,
		                                                      IEnumerable<IXunitTestCase> testCases)
			=> new TestClassRunner(testClass, @class, testCases, diagnosticMessageSink, MessageBus, TestCaseOrderer,
			                       new ExceptionAggregator(Aggregator), CancellationTokenSource, CollectionFixtureMappings)
				.RunAsync();
	}

	sealed class TestClassRunner : XunitTestClassRunner
	{
		public TestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases,
		                       IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer,
		                       ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource,
		                       IDictionary<Type, object> collectionFixtureMappings)
			: base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator,
			       cancellationTokenSource, collectionFixtureMappings) {}

		protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method,
		                                                       IEnumerable<IXunitTestCase> testCases,
		                                                       object[] constructorArguments)
			=> new TestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus,
			                        new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments)
				.RunAsync();
	}

	sealed class TestMethodRunner : XunitTestMethodRunner
	{
		readonly IMessageSink _diagnosticMessageSink;
		readonly object[]     _constructorArguments;

		public TestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method,
		                        IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink,
		                        IMessageBus messageBus, ExceptionAggregator aggregator,
		                        CancellationTokenSource cancellationTokenSource, object[] constructorArguments)
			: base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource,
			       constructorArguments)
		{
			_diagnosticMessageSink = diagnosticMessageSink;
			_constructorArguments  = constructorArguments;
		}

		protected override Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
			=> Runner(testCase)?.RunAsync() ?? base.RunTestCaseAsync(testCase);

		XunitTestCaseRunner Runner(IXunitSerializable testCase)
		{
			switch (testCase)
			{
				case ExecutionErrorTestCase _:
					break;
				case XunitTheoryTestCase @case:
					return new TheoryTestCaseRunner(@case, @case.DisplayName, @case.SkipReason, _constructorArguments,
					                                _diagnosticMessageSink, MessageBus, new ExceptionAggregator(Aggregator),
					                                CancellationTokenSource);
				case XunitTestCase @case:
					return new TestCaseRunner(@case, @case.DisplayName, @case.SkipReason, _constructorArguments,
					                          @case.TestMethodArguments, MessageBus, new ExceptionAggregator(Aggregator),
					                          CancellationTokenSource);
			}

			return null;
		}
	}

	sealed class TheoryTestCaseRunner : XunitTheoryTestCaseRunner
	{
		public TheoryTestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason,
		                            object[] constructorArguments, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
		                            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) :
			base(testCase, displayName, skipReason, constructorArguments, diagnosticMessageSink, messageBus, aggregator,
			     cancellationTokenSource) {}

		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass,
		                                                    object[] constructorArguments,
		                                                    MethodInfo testMethod, object[] testMethodArguments,
		                                                    string skipReason,
		                                                    IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
		                                                    ExceptionAggregator aggregator,
		                                                    CancellationTokenSource cancellationTokenSource)
			=> new TestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			                  skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource);
	}

	sealed class TestCaseRunner : XunitTestCaseRunner
	{
		public TestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason, object[] constructorArguments,
		                      object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator,
		                      CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator,
			       cancellationTokenSource) {}

		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass,
		                                                    object[] constructorArguments,
		                                                    MethodInfo testMethod, object[] testMethodArguments,
		                                                    string skipReason,
		                                                    IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
		                                                    ExceptionAggregator aggregator,
		                                                    CancellationTokenSource cancellationTokenSource)
			=> new TestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			                  skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource);
	}

	sealed class TestRunner : XunitTestRunner
	{
		public TestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
		                  MethodInfo testMethod, object[] testMethodArguments, string skipReason,
		                  IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
		                  CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason,
			       beforeAfterAttributes, aggregator, cancellationTokenSource) {}

		protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
			=> new TestInvoker(Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments,
			                   BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
	}

	sealed class TestInvoker : XunitTestInvoker
	{
		public TestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
		                   MethodInfo testMethod, object[] testMethodArguments,
		                   IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
		                   CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes,
			       aggregator, cancellationTokenSource) {}

		protected override object CallTestMethod(object testClassInstance)
			=> Result(testClassInstance)?
				.ContinueWith(task => task.Exception
				                          .InnerExceptions
				                          .Select(x => x.Demystify())
				                          .ForEach(Aggregator.Add),
				              TaskContinuationOptions.OnlyOnFaulted);

		Task Result(object instance)
		{
			try
			{
				return GetTaskFromResult(base.CallTestMethod(instance));
			}
			catch (Exception e)
			{
				throw e.Demystify();
			}
		}
	}
}