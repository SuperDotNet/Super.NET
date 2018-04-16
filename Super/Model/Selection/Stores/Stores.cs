using Super.ExtensionMethods;
using Super.Reflection.Types;
using System;

namespace Super.Model.Selection.Stores
{
	sealed class Stores<TParameter, TResult> : Select<Func<TParameter, TResult>, ISelect<TParameter, TResult>>
	{
		public static Stores<TParameter, TResult> Default { get; } = new Stores<TParameter, TResult>();

		Stores() :
			base(IsValueType.Default.IsSatisfiedBy(Type<TParameter>.Metadata)
				     ? Selections<TParameter, TResult>.Default.ToDelegate()
				     : new Generic<ISelect<Func<TParameter, TResult>, ISelect<TParameter, TResult>>>(typeof(ReferenceTables<,>))
				       .Get(typeof(TParameter), typeof(TResult))()
				       .Get) {}
	}
}