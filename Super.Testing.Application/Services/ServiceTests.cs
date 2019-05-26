using System;
using FluentAssertions;
using Super.Services;
using Xunit;

namespace Super.Testing.Application.Services
{
	public sealed class ServiceTests
	{
		[Fact]
		void Verify()
		{
			ClientStore.Default.Get(new Uri("http://microsoft.com"))
			           .Should()
			           .BeSameAs(ClientStore.Default.Get(new Uri("http://microsoft.com")));
		}
	}
}