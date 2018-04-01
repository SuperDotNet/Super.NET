using System;
using FluentAssertions;
using Super.Expressions;
using Super.Reflection;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Expressions
{
	public sealed class LambdaTests
	{
		[Fact]
		void Verify()
		{
			Lambda<Func<object>>.Default.Get(Instances.Default.Get(Types<object>.Identity.GetConstructor(Empty<Type>.Array)))
			                    .Compile()()
			                    .Should()
			                    .NotBeNull();
		}
	}
}