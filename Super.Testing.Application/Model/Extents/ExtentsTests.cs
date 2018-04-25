using FluentAssertions;
using Super.Model.Extents;
using Super.Model.Sources;
using Super.Reflection.Types;
using Xunit;

namespace Super.Testing.Application.Model.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifySourceDirect()
		{
			new Source<string>("Hello World!").Out().Type().Metadata().Return().Get().Should().Be(Type<string>.Metadata);
		}

		[Fact]
		void VerifySourceDelegated()
		{
			new Source<int>(6776).Out(x => x.Type().Metadata()).Get().Should().Be(Type<int>.Metadata);
		}

		
	}
}