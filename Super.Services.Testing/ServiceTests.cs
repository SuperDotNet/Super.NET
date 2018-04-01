using FluentAssertions;
using System;
using Xunit;

namespace Super.Services.Testing
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