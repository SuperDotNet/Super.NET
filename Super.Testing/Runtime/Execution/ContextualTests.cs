using FluentAssertions;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Runtime.Execution
{
	public sealed class ContextualTests
	{
		[Fact]
		void Verify()
		{
			var instance = Contextual.Default.Get();
			Contextual.Default.Get().Should().BeSameAs(instance);
			using (ChildExecutionContext.Default.Get("New Context"))
			{
				Contextual.Default.Get().Should().NotBeSameAs(instance);
				Contextual.Default.Get().Should().BeSameAs(Contextual.Default.Get());
			}

			Contextual.Default.Get().Should().BeSameAs(instance);
		}

		sealed class Contextual : Contextual<object>
		{
			public static Contextual Default { get; } = new Contextual();

			Contextual() : base(() => new object()) {}
		}
	}
}