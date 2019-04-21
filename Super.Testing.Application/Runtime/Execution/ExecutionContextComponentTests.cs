using FluentAssertions;
using JetBrains.Annotations;
using Super.Application.Hosting.xUnit;
using Super.Model.Results;
using Super.Runtime.Environment;
using Super.Runtime.Execution;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	[TestCaseOrderer("Super.Application.Hosting.xUnit.PriorityOrderer", "Super.Application.Hosting.xUnit")]
	public sealed class ExecutionContextComponentTests
	{
		[Fact, TestPriority(1)]
		void Verify()
		{
			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("xUnit Testing Application Default (root) Execution Context");
		}

		[Fact, TestPriority(0)]
		void Override()
		{
			Types.Default.Execute(typeof(DefaultExecutionContext).Yield().ToArray());

			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("Local Context");
		}


		[Fact, TestPriority(2)]
		void Empty()
		{
			Types.Default.Execute(typeof(object).Yield().ToArray());

			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("Default Execution Context");
		}

		[Fact]
		void VerifyTest()
		{
			ExecutionContext.Default.Get().Should().BeSameAs(ExecutionContext.Default.Get());
		}

		sealed class DefaultExecutionContext : DelegatedResult<object>, IExecutionContext
		{
			[UsedImplicitly]
			public static IExecutionContext Default { get; } = new DefaultExecutionContext();

			DefaultExecutionContext() : base(() => new ContextDetails("Local Context")) {}
		}
	}
}