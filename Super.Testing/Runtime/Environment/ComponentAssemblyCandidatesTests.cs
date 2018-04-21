using System.Reflection;
using FluentAssertions;
using Super.Runtime.Environment;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
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