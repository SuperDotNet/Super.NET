using Super.ExtensionMethods;
using Super.Reflection;
using System;

namespace Super.Model.Sources
{
	sealed class Sources<TParameter, TResult> : ReferenceStore<Func<TParameter, TResult>, ISource<TParameter, TResult>>
	{
		public static Sources<TParameter, TResult> Default { get; } = new Sources<TParameter, TResult>();

		Sources() : base(x => x.Target as ISource<TParameter, TResult> ??
		                      I<DelegatedSource<TParameter, TResult>>.Default.Get(x)) {}
	}
}