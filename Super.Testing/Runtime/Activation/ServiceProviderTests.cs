using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class ServiceProviderTests
	{
		[Theory]
		[AutoData]
		public void Verify(int number)
		{
			var sut = new ServiceProvider(number);
			sut.IsSatisfiedBy(typeof(DateTime))
			   .Should()
			   .BeFalse();
			sut.IsSatisfiedBy(typeof(int))
			   .Should()
			   .BeTrue();

			sut.Get<int>()
			   .Should()
			   .Be(number);

			sut.Get<DateTime?>()
			   .Should()
			   .BeNull();
		}
	}
}