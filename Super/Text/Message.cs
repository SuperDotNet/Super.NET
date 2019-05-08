using Super.Model.Selection;
using System;

namespace Super.Text
{
	public class Message<T> : Select<T, string>, IMessage<T>
	{
		public Message(Func<T, string> select) : base(select) {}
	}
}