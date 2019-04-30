using FluentAssertions;
using Super.Aspects;
using Super.Compose;
using System;
using Xunit;

namespace Super.Testing.Application.Aspects
{
	public class AssignedAspectTests
	{
		[Fact]
		void Verify()
		{
			var subject = Start.A.Selection<string>().By.Self;
			subject.Invoking(x => x.Get(null)).Should().NotThrow();
			subject.Configured()
			       .Invoking(x => x.Get(null))
			       .Should()
			       .Throw<InvalidOperationException>();
		}

		[Fact]
		void RuntimeRegistration()
		{
			var single    = new Registration(typeof(AssignedAspect<,>));
			var parameter = new[] {A.Type<string>(), A.Type<string>()};
			single.Get(parameter).Should().BeSameAs(AssignedAspect<string, string>.Default);
		}

		/*[Fact]
		void Count()
		{
			var first = 0;
			Enumerable.Range(0, 100)
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .FirstOrDefault();

			first.Should().Be(2);

			var second = 0;

			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .First()
			     .Get(Data.Default.Get());

			second.Should().Be(2);
		}*/
	}
}