using Super.Reflection.Types;
using System;

namespace Super.Model.Selection.Stores
{
	sealed class Stores<TIn, TOut> : Select<Func<TIn, TOut>, ISelect<TIn, TOut>>
	{
		public static Stores<TIn, TOut> Default { get; } = new Stores<TIn, TOut>();

		Stores() :
			base(IsValueType.Default.Get(Type<TIn>.Metadata)
				     ? Selections<TIn, TOut>.Default.ToDelegate()
				     : new Generic<ISelect<Func<TIn, TOut>, ISelect<TIn, TOut>>>(typeof(ReferenceTables<,>))
				       .Get(typeof(TIn), typeof(TOut))()
				       .Get) {}
	}
}