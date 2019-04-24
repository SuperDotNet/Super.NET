using JetBrains.Annotations;
using Super.Runtime.Activation;

namespace Super.Text.Formatting
{
	sealed class DefaultFormatter : Adapter<object>, IActivateUsing<object>
	{
		[UsedImplicitly]
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}