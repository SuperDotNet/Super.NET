using System.Reactive;
using Super.Testing.Objects;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class EnhancedArrayTests
	{
		/*readonly ITestOutputHelper _output;

		public EnhancedArrayTests(ITestOutputHelper output) => _output = output;*/

		[Fact]
		void Verify()
		{
			Data.Default.Get()
			    .ToSource()
			    .Out()
			    .AsSelect()
			    .Iterate()
			    .Selection(x => x.Length, x => x > 1000)
			    .Get(Unit.Default);

			/*var items = new[] {6776};
			var enhanced = new EnhancedArray<int>(items);*/
		}
	}
}