using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Super.Serialization.Testing.Application
{
	public sealed class RangesTests
	{
		readonly ITestOutputHelper _output;

		public RangesTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Verify()
		{
			/*_output.WriteLine($"Low: {Ranges.Default.Low.Start.Value} - {Ranges.Default.Low.End.Value}");
			//_output.WriteLine($"{Ranges.Default.Normal.Start.Value} - {Ranges.Default.Normal.End.Value}");
			_output.WriteLine($"High: {Ranges.Default.High.Start.Value} - {Ranges.Default.High.End.Value}");
			for (var i = Ranges.Default.Normal.Start.Value; i < Ranges.Default.Normal.End.Value; i++)
			{
				_output.WriteLine($"{i}: {(char)i}");
			}*/

			var valid = new string(new[] {'a', 'b', 'c', (char)0xD800, (char)0xDC00});
			var text  = JsonEncodedText.Encode(valid);
			_output.WriteLine(text.ToString());
		}
	}
}