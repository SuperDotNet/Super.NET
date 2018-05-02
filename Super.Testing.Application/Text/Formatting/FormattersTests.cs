using FluentAssertions;
using Super.Text.Formatting;
using System;
using Xunit;

namespace Super.Testing.Application.Text.Formatting
{
	public sealed class FormattersTests
	{
		[Fact]
		void Verify()
		{
			var sut = Formatters.Default.Get(AppDomain.CurrentDomain);
			sut.ToString("F", Provider.Default).Should().Be(AppDomain.CurrentDomain.FriendlyName);
			sut.ToString("I", Provider.Default).Should().Be(AppDomain.CurrentDomain.Id.ToString());
			var @default = DefaultApplicationDomainFormatter.Default.Get(AppDomain.CurrentDomain);
			sut.ToString("x3", Provider.Default).Should().Be(@default);
			sut.ToString("x3", null).Should().Be(@default);
			sut.ToString(string.Empty, null).Should().Be(@default);
			sut.ToString(null, null).Should().Be(@default);
		}
	}
}