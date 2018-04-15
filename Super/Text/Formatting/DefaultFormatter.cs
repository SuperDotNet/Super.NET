namespace Super.Text.Formatting
{
	sealed class DefaultFormatter : Adapter<object>
	{
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}