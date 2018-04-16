using Super.Diagnostics.Logging;
using Super.Text;

namespace Super.Runtime.Execution
{
	sealed class DetailsFormatter : IFormatter<Details>
	{
		public static DetailsFormatter Default { get; } = new DetailsFormatter();

		DetailsFormatter() {}

		public string Get(Details parameter)
			=> $"[{parameter.Observed.ToString(TimestampFormat.Default)}] {parameter.Name}";
	}
}