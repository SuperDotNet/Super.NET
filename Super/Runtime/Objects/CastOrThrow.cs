using Super.Compose;
using Super.Model.Selection;
using Super.Text;
using System;

namespace Super.Runtime.Objects
{
	sealed class CastOrThrow<TFrom, TTo> : DecoratedSelect<TFrom, TTo>
	{
		public static CastOrThrow<TFrom, TTo> Default { get; } = new CastOrThrow<TFrom, TTo>();

		CastOrThrow() : base(new CastOrDefault<TFrom, TTo>(InvalidCast<TFrom, TTo>.Default.Get)) {}
	}

	sealed class InvalidCast<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static InvalidCast<TFrom, TTo> Default { get; } = new InvalidCast<TFrom, TTo>();

		InvalidCast() : this(InvalidCastMessage<TFrom, TTo>.Default) {}

		readonly IMessage<TFrom> _message;

		public InvalidCast(IMessage<TFrom> message) => _message = message;

		public TTo Get(TFrom parameter) => throw new InvalidOperationException(_message.Get(parameter));
	}

	sealed class InvalidCastMessage<TFrom, TTo> : Message<TFrom>
	{
		public static InvalidCastMessage<TFrom, TTo> Default { get; } = new InvalidCastMessage<TFrom, TTo>();

		InvalidCastMessage() :
			base(x => $"Could not cast an object of '{x?.GetType() ?? typeof(TFrom)}' to '{typeof(TTo)}'.") {}
	}

	sealed class CastOrDefault<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static CastOrDefault<TFrom, TTo> Default { get; } = new CastOrDefault<TFrom, TTo>();

		CastOrDefault() : this(Start.A.Selection<TFrom>().By.Default<TTo>().Get) {}

		readonly Func<TFrom, TTo> _default;

		public CastOrDefault(Func<TFrom, TTo> @default) => _default = @default;

		public TTo Get(TFrom parameter) => parameter is TTo to ? to : _default(parameter);
	}
}