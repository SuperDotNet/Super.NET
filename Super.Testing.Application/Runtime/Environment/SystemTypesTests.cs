using FluentAssertions;
using Super.Runtime.Environment;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class SystemTypesTests
	{
		[Fact]
		void Verify()
		{
			SystemTypes.Default.Execute(GetType());

			SystemTypes.Default.Get().Should().HaveCount(1);
			SystemTypes.Default.Execute(default);

			SystemTypes.Default.Get().Should().HaveCountGreaterThan(1);
		}
	}
}