using System;
using Super.Runtime.Activation;

namespace Super.Model.Selection.Stores
{
	public class ReferenceStore<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
		where TParameter : class
	{
		protected ReferenceStore() : this(Activation<TParameter, TResult>.Default.ToDelegate()) {}

		protected ReferenceStore(Func<TParameter, TResult> source)
			: base(ReferenceTables<TParameter, TResult>.Default.Get(source)) {}
	}
}