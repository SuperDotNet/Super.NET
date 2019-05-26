namespace Super.Diagnostics.Logging
{
	public sealed class TimestampFormat : Text.Text
	{
		public static TimestampFormat Default { get; } = new TimestampFormat();

		TimestampFormat() : base("HH:mm:ss:fff") {}
	}
}