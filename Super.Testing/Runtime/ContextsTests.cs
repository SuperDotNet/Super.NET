using FluentAssertions;
using Super.Runtime.Execution;
using System;
using Xunit;

namespace Super.Testing.Runtime
{
	public sealed class ContextsTests
	{
		readonly object _context;

		public ContextsTests() : this(ExecutionContext.Default.Get()) {}

		ContextsTests(object context) => _context = context;

		[Fact]
		void Verify()
		{
			var contexts = Implementations.Contexts.Get();
			contexts.Should().BeSameAs(Implementations.Contexts.Get());
			var current = Implementations.Contexts.Get().Get().To<Context>();
			ExecutionContext.Default.Get().Should().BeSameAs(current);

			ExecutionContext.Default.Get().Should().BeSameAs(_context);

			contexts.Invoking(x => x.Execute()).Should().Throw<InvalidOperationException>();
		}

		[Fact]
		void References()
		{
			Ambient.For<Contexts>().Should().BeSameAs(Ambient.For<Contexts>());
			Ambient.For<ChildContexts>().Should().BeSameAs(Ambient.For<ChildContexts>());
		}
	}
}