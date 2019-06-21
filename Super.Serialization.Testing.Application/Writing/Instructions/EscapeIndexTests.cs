using Xunit;
using Xunit.Abstractions;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class EscapeIndexTests
	{
		readonly ITestOutputHelper _output;

		public EscapeIndexTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Verify()
		{
			_output.WriteLine(((int)char.MaxValue).ToString());
		}
	}
}