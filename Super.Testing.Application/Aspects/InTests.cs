using FluentAssertions;
using JetBrains.Annotations;
using Super.Application.Hosting.xUnit;
using Super.Model.Selection;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Application.Aspects
{
	public sealed class InTests
	{
		[Theory, AutoData]
		void Verify(I<Subject> sut)
		{
			sut.Source(x => x).Should().BeSameAs(I<Subject, int, int>.Default);
		}

		sealed class Subject : ISelect<int, int>
		{
			[UsedImplicitly]
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get(int parameter) => 0;
		}
	}
}