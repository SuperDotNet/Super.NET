using System;
using Super.ExtensionMethods;

namespace Super.Model.Instances
{
	public class Conditional<T> : IInstance<T>
	{
		readonly Func<T>    _source, _fallback;
		readonly Func<bool> _specification;

		public Conditional(Func<bool> specification, IInstance<T> instance, IInstance<T> fallback)
			: this(specification, instance.ToDelegate(), fallback.ToDelegate()) {}

		public Conditional(Func<bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get() => _specification() ? _source() : _fallback();
	}
}