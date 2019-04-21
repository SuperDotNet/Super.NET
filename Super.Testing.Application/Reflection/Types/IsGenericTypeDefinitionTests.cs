using FluentAssertions;
using Super.Reflection.Types;
using Xunit;

namespace Super.Testing.Application.Reflection.Types
{
	public sealed class IsGenericTypeDefinitionTests
	{
		[Fact]
		void Verify()
		{
			var sut = IsGenericTypeDefinition.Default;
			sut.Get(typeof(Subject<object>)).Should().BeFalse();
			sut.Get(typeof(Subject<>)).Should().BeTrue();
		}

		// ReSharper disable once UnusedTypeParameter
		sealed class Subject<T> {}
	}
}