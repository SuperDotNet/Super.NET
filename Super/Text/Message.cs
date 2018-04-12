using Super.Model.Selection;
using System;

namespace Super.Text
{
	public class Message<T> : Delegated<T, string>, IMessage<T>
	{
		public Message(Func<T, string> source) : base(source) {}
	}
}