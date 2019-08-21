namespace Super.Diagnostics.Logging.Text
{
	public sealed class TimestampFormat : Super.Text.Text
	{
		public static TimestampFormat Default { get; } = new TimestampFormat();

		TimestampFormat() : base("HH:mm:ss:fff") {}
	}
}