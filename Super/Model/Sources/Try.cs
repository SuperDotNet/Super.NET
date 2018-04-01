using System;

namespace Super.Model.Sources
{
	public class Try<TException, TParameter, TResult> : ISource<TParameter, TResult> where TException : Exception
	{
		readonly Func<TParameter, TResult> _fallback;
		readonly Func<TParameter, TResult> _source;

		public Try(Func<TParameter, TResult> source, Func<TParameter, TResult> fallback)
		{
			_source   = source;
			_fallback = fallback;
		}

		public TResult Get(TParameter parameter)
		{
			try
			{
				var source = _source(parameter);
				return source;
			}
			catch (TException)
			{
				return _fallback(parameter);
			}
		}
	}
}