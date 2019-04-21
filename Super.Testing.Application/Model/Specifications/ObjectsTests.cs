using FluentAssertions;
using System;
using System.Reflection;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class ObjectsTests
	{
		[Fact]
		public void Coverage()
		{
			var typeInfo = GetType().GetTypeInfo();
			Always<Type>.Default.Get(typeInfo).Should().BeTrue();
		}
	}
}