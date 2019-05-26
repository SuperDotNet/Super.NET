using System;
using Super.Model.Selection;

namespace Super.Text
{
	public class Message<T> : Select<T, string>, IMessage<T>
	{
		public Message(Func<T, string> select) : base(select) {}
	}
}