using System;
using System.Reflection;
using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class ObjectsTests
	{
		[Fact]
		public void Coverage()
		{
			var typeInfo = GetType().GetTypeInfo();
			AlwaysSpecification<Type>.Default.IsSatisfiedBy(typeInfo).Should().BeTrue();
		}
	}
}