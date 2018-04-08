using System;
using Super.ExtensionMethods;
using Super.Reflection;

namespace Super.Model.Selection.Stores
{
	sealed class Stores<TParameter, TResult> : Delegated<Func<TParameter, TResult>, ISelect<TParameter, TResult>>
	{
		public static Stores<TParameter, TResult> Default { get; } = new Stores<TParameter, TResult>();

		Stores() :
			base(IsValueTypeSpecification.Default.IsSatisfiedBy(Types<TParameter>.Key)
				     ? Selections<TParameter, TResult>.Default.ToDelegate()
				     : new Generic<ISelect<Func<TParameter, TResult>, ISelect<TParameter, TResult>>>(typeof(ReferenceTables<,>))
				       .Get(typeof(TParameter), typeof(TResult))()
				       .Get) {}
	}
}