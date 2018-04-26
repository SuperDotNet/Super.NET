using FluentAssertions;
using Super.Application.Hosting.xUnit;
using Super.Runtime.Execution;
using Super.Testing.Objects;
using System.Threading.Tasks;
using Xunit;

namespace Super.Testing.Application.Runtime
{
	public sealed class ContextsTests
	{
		[Theory, AutoData]
		void VerifyChildContext(string name)
		{
			AssignedContext.Default.IsSatisfiedBy().Should().BeFalse();
			var resource = Resource.Default.Get();
			AssignedContext.Default.IsSatisfiedBy().Should().BeTrue();
			Resource.Default.Get().Should().BeSameAs(resource);

			var instance = Contextual.Default.Get();
			Contextual.Default.Get().Should().BeSameAs(instance);

			var child = Child(resource, instance, name);

			var reverted = Resource.Default.Get();
			reverted.Should().NotBeSameAs(child).And.Subject.Should().BeSameAs(resource);
			Contextual.Default.Get().Should().BeSameAs(instance);
			resource.Get().Should().Be(0);
			child.Get().Should().Be(1);
			AssignedContext.Default.IsSatisfiedBy().Should().BeTrue();
			DisposeContext.Default.Execute();
			resource.Get().Should().Be(1);
			AssignedContext.Default.IsSatisfiedBy().Should().BeFalse();
		}


		static CountingDisposable Child(object resource, object instance, string name)
		{
			using (Contexts.Default.Get(name))
			{
				Contextual.Default.Get().Should().NotBeSameAs(instance);

				var result = Resource.Default.Get();

				result.Should().NotBeSameAs(resource).And.Subject.Should().BeSameAs(Resource.Default.Get());
				ExecutionContext.Default.Get().To<ContextDetails>().Details.Name.Should().Be(name);
				result.Get().Should().Be(0);
				return result;
			}
		}

		sealed class Resource : Contextual<CountingDisposable>
		{
			public static Resource Default { get; } = new Resource();

			Resource() : base(() => new CountingDisposable()) {}
		}

		sealed class Contextual : Contextual<CountingDisposable>
		{
			public static Contextual Default { get; } = new Contextual();

			Contextual() : base(() => new CountingDisposable()) {}
		}


		[Fact]
		void VerifyInnerToExternal()
		{
			Task.Run(() => Resource.Default.Get()).Result.Should().NotBeSameAs(Resource.Default.Get());
		}

		[Fact]
		void VerifyExternalToInner()
		{
			Resource.Default.Get().Should().BeSameAs(Task.Run(() => Resource.Default.Get()).Result);
		}
	}
}