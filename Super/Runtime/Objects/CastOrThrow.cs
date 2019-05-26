using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	sealed class CastOrThrow<TFrom, TTo> : Select<TFrom, TTo>
	{
		public static CastOrThrow<TFrom, TTo> Default { get; } = new CastOrThrow<TFrom, TTo>();

		CastOrThrow() : base(new CastOrDefault<TFrom, TTo>(InvalidCast<TFrom, TTo>.Default.Get)) {}
	}
}