using Super.Text;

namespace Super.Runtime
{
	sealed class DefaultMessage<TActual, TExpected> : Message<TActual>
	{
		public static DefaultMessage<TActual, TExpected> Default { get; } = new DefaultMessage<TActual, TExpected>();

		DefaultMessage()
			: base(x => $"Expected instance of type {typeof(TExpected)} to be assigned, but an operation using an instance of {x?.GetType() ?? typeof(TActual)} did not produce this.") {}
	}
}