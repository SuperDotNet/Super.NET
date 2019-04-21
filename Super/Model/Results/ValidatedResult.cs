using Super.Model.Selection.Conditions;
using Super.Runtime;
using System;

namespace Super.Model.Results
{
	public class ValidatedResult<T> : IResult<T>
	{
		public static implicit operator T(ValidatedResult<T> source) => source.Get();

		readonly Func<T, bool> _specification;
		readonly Func<T>       _source, _fallback;

		public ValidatedResult(IResult<T> result, IResult<T> fallback) :
			this(IsAssigned<T>.Default, result, fallback) {}

		public ValidatedResult(ICondition<T> specification, IResult<T> result, IResult<T> fallback)
			: this(specification.Get, result.Get, fallback.Get) {}

		public ValidatedResult(Func<T, bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get()
		{
			var source = _source();
			var result = _specification(source) ? source : _fallback();
			return result;
		}
	}

	public class Validated<T> : IResult<T>
	{
		public static implicit operator T(Validated<T> source) => source.Get();

		readonly Func<bool> _specification;
		readonly Func<T>    _source, _fallback;

		public Validated(ICondition specification, IResult<T> result, IResult<T> fallback)
			: this(specification.Get, result.Get, fallback.Get) {}

		public Validated(Func<bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get()
		{
			var source = _specification() ? _source : _fallback;
			var result = source();
			return result;
		}
	}
}