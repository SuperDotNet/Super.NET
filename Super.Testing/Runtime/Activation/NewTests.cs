using AutoFixture.Xunit2;
using FluentAssertions;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class NewTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var subject = Activate<Subject>.New(number);
			subject.Number.Should()
			       .Be(number)
			       .And.Subject.Should()
			       .NotBeSameAs(New<int, Subject>.Default.Get(number));
		}

		[Theory, AutoData]
		public void Default(int number)
		{
			Activate<Subject>.New(number).Number.Should().Be(number);
		}

		[Fact]
		public void References()
		{
			var first = Activate<SubjectWithoutConstructor>.New(6776);
			var second = Activate<SubjectWithoutConstructor>.New(6776);
			first.Should().NotBeSameAs(second);
		}

		[Fact]
		public void NoParameters()
		{
			Activate<SubjectWithoutConstructor>.New(6776).Should().NotBeNull();
		}


		[Theory, AutoData]
		public void MultipleParameters(int number)
		{
			var subject = Activate<SubjectWithMultipleParameters>.New(number);
			subject.Another.Should().Be(4);
			subject.Number.Should().Be(number);
		}

		sealed class Subject
		{
			public Subject(int number)
			{
				Number = number;
			}

			public int Number { get; }
		}

		sealed class SubjectWithoutConstructor {}

		sealed class SubjectWithMultipleParameters
		{
			public SubjectWithMultipleParameters(int number, int another = 4)
			{
				Number = number;
				Another = another;
			}

			public int Number { get; }

			public int Another { get; }
		}
	}
}