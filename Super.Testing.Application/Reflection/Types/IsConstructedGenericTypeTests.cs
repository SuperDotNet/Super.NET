using FluentAssertions;
using Super.Reflection.Types;
using Xunit;
// ReSharper disable UnusedTypeParameter

namespace Super.Testing.Application.Reflection.Types
{
	public sealed class IsConstructedGenericTypeTests
	{
		[Fact]
		void Verify()
		{
			var sut = IsConstructedGenericType.Default;
			sut.IsSatisfiedBy(typeof(Subject<object>)).Should().BeTrue();
			sut.IsSatisfiedBy(typeof(Subject<>)).Should().BeFalse();
		}

		[Fact]
		void VerifyInterface()
		{
			var sut = IsConstructedGenericType.Default;
			sut.IsSatisfiedBy(typeof(ISubject<object>)).Should().BeTrue();
			sut.IsSatisfiedBy(typeof(ISubject<>)).Should().BeFalse();
		}

		sealed class Subject<T> {}

		interface ISubject<T> {}
	}
}