using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	sealed class Selections<TParameter, TResult> : ReferenceStore<Func<TParameter, TResult>, ISelect<TParameter, TResult>>
	{
		public static Selections<TParameter, TResult> Default { get; } = new Selections<TParameter, TResult>();

		Selections() : base(x => x.Target as ISelect<TParameter, TResult> ??
		                      I<Select<TParameter, TResult>>.Default.From(x)) {}
	}
}