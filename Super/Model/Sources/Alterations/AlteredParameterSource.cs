using System;

namespace Super.Model.Sources.Alterations
{
	public sealed class AlteredParameterSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Alter<TParameter>         _alteration;
		readonly Func<TParameter, TResult> _source;

		public AlteredParameterSource(IAlteration<TParameter> alteration, ISource<TParameter, TResult> source) :
			this(alteration.Get, source.Get) {}

		public AlteredParameterSource(Alter<TParameter> alteration, Func<TParameter, TResult> source)
		{
			_alteration = alteration;
			_source     = source;
		}

		public TResult Get(TParameter parameter) => _source(_alteration(parameter));
	}
}