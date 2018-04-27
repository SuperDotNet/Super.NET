using FluentAssertions;
using Super.Model.Collections;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public class SortSelectorTests
	{
		[Fact]
		void Verify()
		{
			SortSelector<Subject>.Default.Get(new Subject()).Should().Be(-1);
		}

		[Fact]
		void VerifyAware()
		{
			SortSelector<Aware>.Default.Get(new Aware()).Should().Be(6776);
		}

		[Fact]
		void VerifyDeclared()
		{
			SortSelector<Declared>.Default.Get(new Declared()).Should().Be(123);
		}

		sealed class Subject {}

		sealed class Aware : ISortAware
		{
			public int Get() => 6776;
		}

		[Sort(123)]
		sealed class Declared
		{

		}
	}
}