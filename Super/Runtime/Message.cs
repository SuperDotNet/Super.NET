using System;
using Super.Model.Selection;
using Super.Text;

namespace Super.Runtime
{
	class Message<T> : Delegated<T, string>, IMessage<T>
	{
		public Message(Func<T, string> source) : base(source) {}
	}
}