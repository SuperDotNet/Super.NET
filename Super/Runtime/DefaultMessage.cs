namespace Super.Runtime
{
	sealed class DefaultMessage<TParameter, TResult> : Message<TParameter>
	{
		public static DefaultMessage<TParameter, TResult> Default { get; } = new DefaultMessage<TParameter, TResult>();

		DefaultMessage() :
			base(x => $"Expected instance of type {typeof(TResult)} to be assigned, but an operation using an instance of {x.GetType()} did not produce this.") {}
	}
}