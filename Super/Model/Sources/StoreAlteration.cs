using System;
using Super.ExtensionMethods;
using Super.Model.Sources.Alterations;

namespace Super.Model.Sources
{
	public sealed class StoreAlteration<TParameter, TResult> : IAlteration<Func<TParameter, TResult>>
		where TParameter : class
	{
		public static StoreAlteration<TParameter, TResult> Default { get; } = new StoreAlteration<TParameter, TResult>();

		StoreAlteration() {}

		public Func<TParameter, TResult> Get(Func<TParameter, TResult> parameter)
			=> ReferenceStores<TParameter, TResult>.Default.Get(parameter).ToDelegate();
	}
}