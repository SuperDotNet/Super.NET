using System;
using System.Reflection;
using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class ObjectsTests
	{
		[Fact]
		public void Coverage()
		{
			var typeInfo = GetType().GetTypeInfo();
			Always<Type>.Default.IsSatisfiedBy(typeInfo).Should().BeTrue();
		}
	}
}