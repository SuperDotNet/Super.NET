using Super.Runtime.Activation;

namespace Super.Text.Formatting
{
	sealed class DefaultFormatter : Adapter<object>, IActivateUsing<object>
	{
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}