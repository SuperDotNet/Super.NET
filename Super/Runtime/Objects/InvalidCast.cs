using System;
using Super.Model.Selection;
using Super.Text;

namespace Super.Runtime.Objects
{
	sealed class InvalidCast<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static InvalidCast<TFrom, TTo> Default { get; } = new InvalidCast<TFrom, TTo>();

		InvalidCast() : this(InvalidCastMessage<TFrom, TTo>.Default) {}

		readonly IMessage<TFrom> _message;

		public InvalidCast(IMessage<TFrom> message) => _message = message;

		public TTo Get(TFrom parameter) => throw new InvalidOperationException(_message.Get(parameter));
	}
}