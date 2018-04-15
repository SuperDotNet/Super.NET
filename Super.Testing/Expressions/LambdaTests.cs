using FluentAssertions;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Invocation.Expressions;
using System;
using Xunit;

namespace Super.Testing.Expressions
{
	public sealed class LambdaTests
	{
		[Fact]
		void Verify()
		{
			Lambda<Func<object>>.Default.Get(Instances.Default.Get(Type<object>.Instance.GetConstructor(Empty<Type>.Array)))
			                    .Compile()()
			                    .Should()
			                    .NotBeNull();
		}
	}
}