using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class EqualitySpecificationTests
	{
		[Fact]
		public void Number()
		{
			var sut = new EqualitySpecification<int>(3);
			sut.IsSatisfiedBy(4)
			   .Should()
			   .BeFalse();
			sut.IsSatisfiedBy(3)
			   .Should()
			   .BeTrue();
		}

		[Fact]
		public void Object()
		{
			var source = new object();
			var sut    = new EqualitySpecification<object>(source);
			sut.IsSatisfiedBy(new object())
			   .Should()
			   .BeFalse();
			sut.IsSatisfiedBy(source)
			   .Should()
			   .BeTrue();
		}
	}
}