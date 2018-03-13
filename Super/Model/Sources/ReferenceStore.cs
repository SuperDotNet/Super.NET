using System;
using Super.ExtensionMethods;
using Super.Model.Sources.Tables;
using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	class ReferenceStore<TParameter, TResult> : DecoratedSource<TParameter, TResult>
		where TParameter : class
	{
		public ReferenceStore() : this(New<TParameter, TResult>.Default.ToDelegate()) {}

		public ReferenceStore(Func<TParameter, TResult> source) :
			base(ReferenceTables<TParameter, TResult>.Default.Get(source)) {}
	}
}