using FluentAssertions;
using Super.Model.Collections;
using Super.Reflection.Types;
using System.Reflection;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public class SortMetadataTests
	{
		[Fact]
		void Verify()
		{
			SortMetadata<Subject>.Default.Get(new Subject()).Should().Be(200);
		}

		[Fact]
		void VerifyMetadata()
		{
			SortMetadata<TypeInfo>.Default.Get(Type<Subject>.Metadata).Should().Be(200);
		}

		[Sort(200)]
		sealed class Subject {}
	}
}