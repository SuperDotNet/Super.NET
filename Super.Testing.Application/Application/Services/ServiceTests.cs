using FluentAssertions;
using Super.Application.Services;
using System;
using Xunit;

namespace Super.Testing.Application.Application.Services
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