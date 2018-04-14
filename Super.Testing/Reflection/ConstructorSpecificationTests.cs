using System;
using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Reflection
{
	public sealed class ConstructorSpecificationTests
	{
		sealed class Optional
		{
			public Optional(int number = 123) {}
		}

		[Fact]
		void Verify()
		{
			ConstructorSpecification.Default.IsSatisfiedBy(Type<object>.Instance.GetConstructor(Empty<Type>.Array));
		}

		[Fact]
		void VerifyOptional()
		{
			ConstructorSpecification.Default.IsSatisfiedBy(Type<Optional>.Instance.GetConstructors().Only());
		}
	}
}