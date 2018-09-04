using JetBrains.Annotations;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Runtime.Environment;
using Super.Runtime.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

// ReSharper disable All

namespace Super.Application.Hosting.xUnit
{
	public sealed class DefaultExecutionContext : DelegatedSource<object>, IExecutionContext
	{
		public static IExecutionContext Default { get; } = new DefaultExecutionContext();

		DefaultExecutionContext() :
			base(() => new ContextDetails("xUnit Testing Application Default (root) Execution Context")) {}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class TestPriorityAttribute : Attribute
	{
		public TestPriorityAttribute(int priority)
		{
			Priority = priority;
		}

		public int Priority { get; private set; }
	}

	public class PriorityOrderer : ITestCaseOrderer
	{
		public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
			where TTestCase : ITestCase
		{
			var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

			foreach (TTestCase testCase in testCases)
			{
				int priority = 0;

				foreach (IAttributeInfo attr in
					testCase.TestMethod.Method.GetCustomAttributes((typeof(TestPriorityAttribute)
							                                               .AssemblyQualifiedName)))
					priority = attr.GetNamedArgument<int>("Priority");

				GetOrCreate(sortedMethods, priority).Add(testCase);
			}

			foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
			{
				list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name,
				                                                             y.TestMethod.Method.Name));
				foreach (TTestCase testCase in list)
					yield return testCase;
			}
		}

		static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
		{
			TValue result;

			if (dictionary.TryGetValue(key, out result)) return result;

			result          = new TValue();
			dictionary[key] = result;

			return result;
		}
	}

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

		protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases,
		                                           IMessageSink executionMessageSink,
		                                           ITestFrameworkExecutionOptions executionOptions)
		{
			using (var assemblyRunner = new AssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink,
			                                               executionMessageSink,
			                                               executionOptions))
			{
				StorageTypeDefinition.Default.Execute(typeof(Logical<>));
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
			                       new ExceptionAggregator(Aggregator), CancellationTokenSource,
			                       CollectionFixtureMappings)
				.RunAsync();
	}

	sealed class TestClassRunner : XunitTestClassRunner
	{
		public TestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases,
		                       IMessageSink diagnosticMessageSink, IMessageBus messageBus,
		                       ITestCaseOrderer testCaseOrderer,
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

	sealed class Decorated : IMethodInfo
	{
		readonly IMethodInfo _method;

		public Decorated(IMethodInfo method)
		{
			_method = method;
		}

		public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
		{
			try
			{
				return _method.GetCustomAttributes(assemblyQualifiedAttributeTypeName);
			}
			catch (Exception e)
			{
				throw Unwrap(e).Demystify();
			}
		}

		static Exception Unwrap(Exception ex)
		{
			while (true)
			{
				var tiex = ex as TargetInvocationException;
				if (tiex == null)
					return ex;

				ex = tiex.InnerException;
			}
		}

		public IEnumerable<ITypeInfo> GetGenericArguments()
		{
			return _method.GetGenericArguments();
		}

		public IEnumerable<IParameterInfo> GetParameters()
		{
			return _method.GetParameters();
		}

		public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments)
		{
			return _method.MakeGenericMethod(typeArguments);
		}

		public bool IsAbstract => _method.IsAbstract;

		public bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

		public bool IsPublic => _method.IsPublic;

		public bool IsStatic => _method.IsStatic;

		public string Name => _method.Name;

		public ITypeInfo ReturnType => _method.ReturnType;

		public ITypeInfo Type => _method.Type;
	}

	sealed class TestMethod : ITestMethod
	{
		readonly ITestMethod _method;

		public TestMethod() {}

		public TestMethod(ITestMethod method) : this(method, new Decorated(method.Method)) {}

		public TestMethod(ITestMethod method, IMethodInfo info)
		{
			_method = method;
			Method   = info;
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			_method.Deserialize(info);
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			_method.Serialize(info);
		}

		public IMethodInfo Method { get; }

		public ITestClass TestClass => _method.TestClass;
	}

	sealed class TestMethodRunner : XunitTestMethodRunner
	{
		readonly IMessageSink _diagnosticMessageSink;
		readonly object[]     _constructorArguments;

		public TestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method,
		                        IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink,
		                        IMessageBus messageBus, ExceptionAggregator aggregator,
		                        CancellationTokenSource cancellationTokenSource, object[] constructorArguments)
			: base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator,
			       cancellationTokenSource,
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
					                                _diagnosticMessageSink, MessageBus,
					                                new ExceptionAggregator(Aggregator),
					                                CancellationTokenSource);
				case XunitTestCase @case:
					return new TestCaseRunner(@case, @case.DisplayName, @case.SkipReason, _constructorArguments,
					                          @case.TestMethodArguments, MessageBus,
					                          new ExceptionAggregator(Aggregator),
					                          CancellationTokenSource);
			}

			return null;
		}
	}

	sealed class TestCase : LongLivedMarshalByRefObject, IXunitTestCase
	{
		readonly IXunitTestCase _case;
		readonly ITestMethod    _method;
		readonly ISpecification _specification;
		readonly Action         _action;

		public TestCase() {}

		public TestCase(IXunitTestCase @case, Action action) : this(@case, new TestMethod(@case.TestMethod),
		                                                            new First(), action) {}

		public TestCase(IXunitTestCase @case, ITestMethod method, ISpecification specification, Action action)
		{
			_case          = @case;
			_method        = method;
			_specification = specification;
			_action        = action;
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			_case.Deserialize(info);
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			_case.Serialize(info);
		}

		public string DisplayName => _case.DisplayName;

		public string SkipReason => _case.SkipReason;

		public ISourceInformation SourceInformation
		{
			get => _case.SourceInformation;
			set => _case.SourceInformation = value;
		}

		public ITestMethod TestMethod
		{
			get
			{
				if (_specification.IsSatisfiedBy())
				{
					_action();
					return _method;
				}

				return _case.TestMethod;
			}
		}

		public object[] TestMethodArguments => _case.TestMethodArguments;

		public Dictionary<string, List<string>> Traits => _case.Traits;

		public string UniqueID => _case.UniqueID;

		public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus,
		                                 object[] constructorArguments,
		                                 ExceptionAggregator aggregator,
		                                 CancellationTokenSource cancellationTokenSource)
			=> _case.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator,
			                  cancellationTokenSource);

		public Exception InitializationException => _case.InitializationException;

		public IMethodInfo Method => _method.Method;
		public int Timeout => _case.Timeout;
	}

	sealed class TheoryTestCaseRunner : XunitTheoryTestCaseRunner
	{
		public TheoryTestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason,
		                            object[] constructorArguments, IMessageSink diagnosticMessageSink,
		                            IMessageBus messageBus,
		                            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) :
			base(testCase, displayName, skipReason, constructorArguments, diagnosticMessageSink, messageBus, aggregator,
			     cancellationTokenSource) {}

		protected override async Task AfterTestCaseStartingAsync()
		{
			var @case = TestCase;
			TestCase = new TestCase(@case, (() => TestCase = @case));
			await base.AfterTestCaseStartingAsync();
		}

		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass,
		                                                    object[] constructorArguments,
		                                                    MethodInfo testMethod, object[] testMethodArguments,
		                                                    string skipReason,
		                                                    IReadOnlyList<BeforeAfterTestAttribute>
			                                                    beforeAfterAttributes,
		                                                    ExceptionAggregator aggregator,
		                                                    CancellationTokenSource cancellationTokenSource)
			=> new TestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			                  skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator),
			                  cancellationTokenSource);
	}

	sealed class TestCaseRunner : XunitTestCaseRunner
	{
		public TestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason,
		                      object[] constructorArguments,
		                      object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator,
		                      CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator,
			       cancellationTokenSource) {}

		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass,
		                                                    object[] constructorArguments,
		                                                    MethodInfo testMethod, object[] testMethodArguments,
		                                                    string skipReason,
		                                                    IReadOnlyList<BeforeAfterTestAttribute>
			                                                    beforeAfterAttributes,
		                                                    ExceptionAggregator aggregator,
		                                                    CancellationTokenSource cancellationTokenSource)
			=> new TestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			                  skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator),
			                  cancellationTokenSource);
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
		                   IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
		                   ExceptionAggregator aggregator,
		                   CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			       beforeAfterAttributes,
			       aggregator, cancellationTokenSource) {}

		protected override object CallTestMethod(object testClassInstance)
			=> Result(testClassInstance)
				?
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