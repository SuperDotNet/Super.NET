using FluentAssertions;
using Super.Application.Testing;
using Super.ExtensionMethods;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Runtime.Execution
{
	public sealed class ContextualTests
	{
		[Theory, AutoData]
		void Verify(string name)
		{
			var instance = Contextual.Default.Get();
			Contextual.Default.Get().Should().BeSameAs(instance);
			using (ChildExecutionContext.Default.Get(name))
			{
				Contextual.Default.Get().Should().NotBeSameAs(instance);
				var child = Contextual.Default.Get();
				child.Should().BeSameAs(Contextual.Default.Get());
				ExecutionContext.Default.Get().To<Context>().Details.Name.Should().Be(name);
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