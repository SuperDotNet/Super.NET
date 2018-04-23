using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection.Stores
{
	public class ReferenceStore<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
		where TParameter : class
	{
		readonly static Func<TParameter, TResult> Source = Activation<TParameter, TResult>.Default.ToDelegate();

		protected ReferenceStore() : this(Source) {}

		protected ReferenceStore(Func<TParameter, TResult> source) 
			: base(ReferenceTables<TParameter, TResult>.Default.Get(source)) {}
	}
}