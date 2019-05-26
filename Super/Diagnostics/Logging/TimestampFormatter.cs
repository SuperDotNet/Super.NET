using Super.Text.Formatting;

namespace Super.Diagnostics.Logging
{
	public sealed class TimestampFormatter : DateTimeOffsetFormatter
	{
		public static TimestampFormatter Default { get; } = new TimestampFormatter();

		TimestampFormatter() : base(TimestampFormat.Default) {}
	}
}