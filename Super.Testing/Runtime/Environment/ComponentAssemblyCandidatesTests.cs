using FluentAssertions;
using Super.Runtime.Environment;
using System.Reflection;
using Xunit;

namespace Super.Testing.Runtime.Environment
{
	public sealed class ComponentAssemblyCandidatesTests
	{
		[Fact]
		void Verify()
		{
			ComponentAssemblyCandidates.Default
			                           .Get(new AssemblyName("Super.Duper.Awesome.Namespace.Application"))
			                           .Should()
			                           .BeEquivalentTo(new AssemblyName("Super.Duper.Awesome.Namespace.Application"),
			                                           new AssemblyName("Super.Duper.Awesome.Namespace"),
			                                           new AssemblyName("Super.Duper.Awesome"),
			                                           new AssemblyName("Super.Duper"),
			                                           new AssemblyName("Super"));
		}
	}
}