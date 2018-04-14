using FluentAssertions;
using Super.Reflection.Assemblies;
using Super.Runtime.Environment;
using System.Reflection;
using Xunit;

namespace Super.Testing.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetailsTests
	{
		[Fact]
		void Verify()
		{
			var details = PrimaryAssemblyDetails.Default.Get();
			var assembly = Assembly.GetExecutingAssembly();
			details.Version.Should().BeEquivalentTo(assembly.GetName().Version);
			details.Title.Should().NotBeNull().And.Be(AssemblyTitle.Default.Get(assembly));
		}
	}
}