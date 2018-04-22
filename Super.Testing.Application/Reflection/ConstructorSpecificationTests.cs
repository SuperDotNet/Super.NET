using System;
using System.Reflection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Reflection
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
			ConstructorSpecification.Default.IsSatisfiedBy(Type<Optional>.Instance.GetConstructors().Only<ConstructorInfo>());
		}
	}
}