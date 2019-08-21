using Super.Text.Formatting;

namespace Super.Diagnostics.Logging.Text
{
	public sealed class TimestampFormatter : DateTimeOffsetFormatter
	{
		public static TimestampFormatter Default { get; } = new TimestampFormatter();

		TimestampFormatter() : base(TimestampFormat.Default) {}
	}
}