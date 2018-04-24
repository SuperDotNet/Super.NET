using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	sealed class Cast<TFrom, TTo> : DecoratedSelect<TFrom, TTo>
	{
		public static Cast<TFrom, TTo> Default { get; } = new Cast<TFrom, TTo>();

		Cast() : base(CanCast<TFrom, TTo>.Default
		                                 .If(CastSelector<TFrom, TTo>.Default, Default<TFrom, TTo>.Instance)) {}
	}
}