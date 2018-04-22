using FluentAssertions;
using Super.Runtime;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class ContextualTests
	{
		/*readonly Log<ContextsTests> _log;

		public ContextualTests() : this(Log<ContextsTests>.Default) {}

		ContextualTests(Log<ContextsTests> log) => _log = log;*/

		[Fact]
		void Verify()
		{
			AssignedContext.Default.IsSatisfiedBy().Should().BeFalse();
			var instance = Contextual.Default.Get();
			AssignedContext.Default.IsSatisfiedBy().Should().BeTrue();
			Contextual.Default.Get().Should().BeSameAs(instance); // TODO:
			/*using (ExecutionContexts.Default.Get(name))
			{
				Contextual.Default.Get().Should().NotBeSameAs(instance);
				var child = Contextual.Default.Get();
				child.Should().BeSameAs(Contextual.Default.Get());
				ExecutionContext.Default.Get().To<ContextDetails>().Details.Name.Should().Be(name);
			}*/

			Contextual.Default.Get().Should().BeSameAs(instance);
		}

		[Fact]
		void VerifyResource()
		{
			AssignedContext.Default.IsSatisfiedBy().Should().BeFalse();
			AssignedContext.Default.Get().Should().BeNull();
			var instance = Resource.Default.Get();
			var context = AssignedContext.Default.Get();
			context.Should().NotBeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeTrue();
			instance.Counter.Get().Should().Be(0);
			AssignedContext.Default.IsSatisfiedBy().Should().BeTrue();
			DisposeContext.Default.Execute();

			instance.Counter.Get().Should().Be(1);
			AssignedContext.Default.Get().Should().BeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeFalse();
		}

		/*[Fact]
		void VerifyInnerToExternal()
		{
			Task.Run(() => Contextual.Default.Get()).Result.Should().NotBeSameAs(Contextual.Default.Get());
		}

		[Fact]
		void VerifyExternalToInner()
		{
			Contextual.Default.Get().Should().BeSameAs(Task.Run(() => Contextual.Default.Get()).Result);
		}*/

		/*[Fact]
		void VerifyLog()
		{
			_log.Get().Should().BeSameAs(_log.Get());
		}*/

		sealed class Resource : ContextualResource<CountingDisposable>
		{
			public static Resource Default { get; } = new Resource();

			Resource() : base(() => new CountingDisposable()) {}
		}

		sealed class CountingDisposable : DelegatedDisposable
		{
			public CountingDisposable() : this(new Counter()) {}

			public CountingDisposable(Counter counter) : base(() => counter.Count()) => Counter = counter;

			public Counter Counter { get; }
		}

		sealed class Contextual : Contextual<object>
		{
			public static Contextual Default { get; } = new Contextual();

			Contextual() : base(() => new object()) {}
		}
	}
}