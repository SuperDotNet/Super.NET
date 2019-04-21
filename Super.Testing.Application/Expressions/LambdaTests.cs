using System;
using FluentAssertions;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Invocation.Expressions;
using Xunit;

namespace Super.Testing.Application.Expressions
{
	public sealed class LambdaTests
	{
		[Fact]
		void Verify()
		{
			Lambda<Func<object>>.Default.Get(ConstructorExpressions.Default.Get(Type<object>.Instance.GetConstructor(Empty<Type>.Array)))
			                    .Compile()()
			                    .Should()
			                    .NotBeNull();
		}
	}
}