using System;

namespace Super.Model.Sources
{
	public class Decorator<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<Decoration<TParameter, TResult>, TResult> _decorator;
		readonly Func<TParameter, TResult>                      _source;

		public Decorator(ISource<Decoration<TParameter, TResult>, TResult> decorator,
		                 ISource<TParameter, TResult> source)
			: this(decorator.Get, source.Get) {}

		public Decorator(Func<Decoration<TParameter, TResult>, TResult> decorator)
			: this(decorator, _ => default) {}

		public Decorator(Func<Decoration<TParameter, TResult>, TResult> decorator,
		                 Func<TParameter, TResult> source)
		{
			_decorator = decorator;
			_source    = source;
		}

		public TResult Get(TParameter parameter)
			=> _decorator(new Decoration<TParameter, TResult>(parameter, _source(parameter)));
	}
}