using Super.ExtensionMethods;
using Super.Model.Sources.Tables;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class ReferenceStore<TParameter, TResult> : DecoratedSource<TParameter, TResult>
		where TParameter : class
	{
		protected ReferenceStore() : this(Activation<TParameter, TResult>.Default.ToDelegate()) {}

		protected ReferenceStore(Func<TParameter, TResult> source)
			: base(ReferenceTables<TParameter, TResult>.Default.Get(source)) {}
	}
}