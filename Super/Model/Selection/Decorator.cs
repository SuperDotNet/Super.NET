using System;

namespace Super.Model.Selection
{
	public class Decorator<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<Decoration<TParameter, TResult>, TResult> _decorator;
		readonly Func<TParameter, TResult>                      _source;

		public Decorator(ISelect<Decoration<TParameter, TResult>, TResult> decorator,
		                 ISelect<TParameter, TResult> @select)
			: this(decorator.Get, @select.Get) {}

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