using FluentAssertions;
using Super.Runtime;
using System.Collections.Immutable;
using Xunit;

namespace Super.Testing.Application.Runtime
{
	public sealed class IsAssignedTests
	{
		[Fact]
		void VerifyReferences()
		{
			IsAssigned<object>.Default.IsSatisfiedBy(new object()).Should().BeTrue();
			IsAssigned<object>.Default.IsSatisfiedBy(null).Should().BeFalse();}

		[Fact]
		void VerifyValues()
		{
			IsAssigned<int>.Default.IsSatisfiedBy(1).Should().BeTrue();
			IsAssigned<int>.Default.IsSatisfiedBy(0).Should().BeFalse();
		}

		[Fact]
		void VerifyUnassignedValues()
		{
			IsAssigned<int?>.Default.IsSatisfiedBy(1).Should().BeTrue();
			IsAssigned<int?>.Default.IsSatisfiedBy(0).Should().BeTrue();
			IsAssigned<int?>.Default.IsSatisfiedBy(null).Should().BeFalse();
		}

		[Fact]
		void VerifyImmutableArrays()
		{
			IsAssigned<ImmutableArray<object>>.Default.IsSatisfiedBy(default).Should().BeFalse();
			IsAssigned<ImmutableArray<object>>.Default.IsSatisfiedBy(ImmutableArray<object>.Empty).Should().BeTrue();
			IsAssigned<ImmutableArray<object>>.Default.IsSatisfiedBy(ImmutableArray.Create(new object())).Should().BeTrue();
		}
	}
}