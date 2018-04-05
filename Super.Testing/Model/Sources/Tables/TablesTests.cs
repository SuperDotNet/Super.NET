using FluentAssertions;
using Super.Model.Sources.Tables;
using Xunit;

namespace Super.Testing.Model.Sources.Tables
{
	public class TablesTests
	{
		[Fact]
		void Verify()
		{
			Tables<string, object>.Default.Get(_ => null).Should().NotBeSameAs(Tables<string, object>.Default.Get(_ => null));
		}
	}
}