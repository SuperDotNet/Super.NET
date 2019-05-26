using System.Reflection;
using FluentAssertions;
using Super.Model.Sequences.Collections;
using Super.Reflection.Types;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Collections
{
	public class SortMetadataTests
	{
		[Sort(200)]
		sealed class Subject {}

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
	}
}