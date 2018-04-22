using FluentAssertions;
using Super.Runtime.Environment;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class ComponentTypeCandidatesTests
	{
		[Fact]
		void Verify()
		{
			ComponentTypeCandidatesStore.Default.Execute(GetType());

			ComponentTypeCandidates.Default.Get().Should().HaveCount(1);
			ComponentTypeCandidatesStore.Default.Execute(default);

			ComponentTypeCandidates.Default.Get().Should().HaveCountGreaterThan(1);
		}
	}
}