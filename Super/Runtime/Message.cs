using Super.Model.Sources;
using System;

namespace Super.Runtime
{
	class Message<T> : DelegatedSource<T, string>, IMessage<T>
	{
		public Message(Func<T, string> source) : base(source) {}
	}
}