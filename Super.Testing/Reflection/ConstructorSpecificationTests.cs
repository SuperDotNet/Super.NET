using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime;
using System;
using Xunit;

namespace Super.Testing.Reflection
{
	public sealed class ConstructorSpecificationTests
	{
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

		sealed class Optional
		{
			public Optional(int number = 123) {}
		}
	}
}