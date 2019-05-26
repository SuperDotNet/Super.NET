using System;
using System.Reflection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Reflection
{
	public sealed class ConstructorConditionTests
	{
		sealed class Optional
		{
			public Optional(int number = 123) {}
		}

		[Fact]
		void Verify()
		{
			ConstructorCondition.Default.Get(Type<object>.Instance.GetConstructor(Empty<Type>.Array));
		}

		[Fact]
		void VerifyOptional()
		{
			ConstructorCondition.Default.Get(Type<Optional>.Instance.GetConstructors().Only<ConstructorInfo>());
		}
	}
}