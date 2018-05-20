using System;

namespace Super.Model.Sources
{
	public class Conditional<T> : ISource<T>
	{
		readonly Func<T>    _source, _fallback;
		readonly Func<bool> _specification;

		public Conditional(Func<bool> specification, ISource<T> source, ISource<T> fallback)
			: this(specification, source.Get, fallback.Get) {}

		public Conditional(Func<bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get() => _specification() ? _source() : _fallback();

		public static implicit operator T(Conditional<T> source) => source.Get();
	}
}