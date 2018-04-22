using FluentAssertions;
using Super.Model.Selection.Stores;
using Xunit;

namespace Super.Testing.Application.Model.Sources.Tables
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