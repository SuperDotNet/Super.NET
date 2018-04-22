using FluentAssertions;
using Super.Application.Hosting.xUnit;
using Super.Diagnostics.Logging;
using Super.Runtime.Execution;
using System.Threading.Tasks;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class ContextualTests
	{
		readonly Log<ContextsTests> _log;

		public ContextualTests() : this(Log<ContextsTests>.Default) {}

		ContextualTests(Log<ContextsTests> log) => _log = log;

		[Theory, AutoData]
		void Verify(string name)
		{
			var instance = Contextual.Default.Get();
			Contextual.Default.Get().Should().BeSameAs(instance);
			using (ExecutionContexts.Default.Get(name))
			{
				Contextual.Default.Get().Should().NotBeSameAs(instance);
				var child = Contextual.Default.Get();
				child.Should().BeSameAs(Contextual.Default.Get());
				ExecutionContext.Default.Get().To<ContextDetails>().Details.Name.Should().Be(name);
			}

			Contextual.Default.Get().Should().BeSameAs(instance);
		}

		[Fact]
		void VerifyInnerToExternal()
		{
			Task.Run(() => Contextual.Default.Get()).Result.Should().NotBeSameAs(Contextual.Default.Get());
		}

		[Fact]
		void VerifyExternalToInner()
		{
			Contextual.Default.Get().Should().BeSameAs(Task.Run(() => Contextual.Default.Get()).Result);
		}

		[Fact]
		void VerifyLog()
		{
			_log.Get().Should().BeSameAs(_log.Get());
		}

		sealed class Contextual : Contextual<object>
		{
			public static Contextual Default { get; } = new Contextual();

			Contextual() : base(() => new object()) {}
		}
	}
}