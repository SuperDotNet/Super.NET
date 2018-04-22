using FluentAssertions;
using Super.Application.Hosting.xUnit;
using Super.Model.Selection;
using Super.Reflection;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Aspects
{
	public sealed class InTests
	{
		[Theory, AutoData]
		void Verify(I<Subject> sut)
		{
			sut.Source<Subject, int, int>(x => x).Should().BeSameAs(I<Subject, int, int>.Default);
		}

		sealed class Subject : ISelect<int, int>
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get(int parameter) => 0;
		}
	}
}