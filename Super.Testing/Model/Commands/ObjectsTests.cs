using System.Collections.Immutable;
using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Model.Commands
{
	public class ObjectsTests
	{
		[Fact]
		public void Execute()
		{
			var count   = 0;
			var command = new DelegatedCommand<(int, int)>(tuple => count = tuple.Item1 + tuple.Item2);
			command.Execute(1, 2);
			count.Should()
			     .Be(3);
		}

		[Fact]
		public void ExecuteItems()
		{
			var count   = 0;
			var command = new DelegatedCommand<ImmutableArray<int>>(items => count = items.Length);
			command.Execute(1, 2, 3, 4, 5, 6);
			count.Should()
			     .Be(6);
		}

		[Fact]
		public void ExecuteTriple()
		{
			var count   = 0;
			var command = new DelegatedCommand<(int, int, int)>(tuple => count = tuple.Item1 + tuple.Item2 + tuple.Item3);
			command.Execute(1, 2, 3);
			count.Should()
			     .Be(6);
		}
	}
}