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
			ConstructorSpecification.Default.IsSatisfiedBy(Types<object>.Identity.GetConstructor(Empty<Type>.Array));
		}

		[Fact]
		void VerifyOptional()
		{
			ConstructorSpecification.Default.IsSatisfiedBy(Types<Optional>.Identity.GetConstructors().Only());
		}
	}
}