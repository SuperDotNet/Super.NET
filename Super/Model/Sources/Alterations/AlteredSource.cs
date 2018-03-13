using System;

namespace Super.Model.Sources.Alterations
{
	public sealed class AlteredSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TResult, TResult>    _alteration;
		readonly Func<TParameter, TResult> _source;

		public AlteredSource(Func<TParameter, TResult> source, Func<TResult, TResult> alteration)
		{
			_alteration = alteration;
			_source     = source;
		}

		public TResult Get(TParameter parameter) => _alteration(_source(parameter));
	}
}