using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class Message<TParameter, TResult> : DelegatedSource<TParameter, string>, IMessage<TParameter>
	{
		public static Message<TParameter, TResult> Default { get; } = new Message<TParameter, TResult>();

		Message() :
			this(x => $"Expected instance of type {typeof(TResult)} to be assigned, but an operation using an instance of {x.GetType()} did not produce this.") {}

		public Message(Func<TParameter, string> source) : base(source) {}
	}
}