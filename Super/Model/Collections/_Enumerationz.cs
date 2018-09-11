using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class OnlyElement<T> : ISelect<IEnumerable<T>, T>, IActivateMarker<Func<T, bool>>
	{
		public static OnlyElement<T> Default { get; } = new OnlyElement<T>();

		OnlyElement() : this(Always<T>.Default.IsSatisfiedBy) {}

		readonly Func<T, bool> _where;

		public OnlyElement(Func<T, bool> where) => _where = where;

		public T Get(IEnumerable<T> parameter)
		{
			var enumerable = parameter.Where(_where).ToArray();
			var result     = enumerable.Length == 1 ? enumerable[0] : default;
			return result;
		}
	}

	public sealed class Only<T> : One<T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.IsSatisfiedBy) {}

		public Only(Func<T, bool> @where) : base(@where) {}
	}


	public sealed class Single<T> : One<T>
	{
		public static Single<T> Default { get; } = new Single<T>();

		Single() : this(Always<T>.Default.IsSatisfiedBy) {}

		public Single(Func<T, bool> @where) : base(@where, () => throw new InvalidOperationException()) {}
	}

	public class One<T> : IReduce<T>, IActivateMarker<Func<T, bool>>
	{
		readonly Func<ReadOnlyMemory<T>, ReadOnlyMemory<T>> _where;
		readonly Func<T> _default;

		public One(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public One(Func<T, bool> where, Func<T> @default) : this(new Where<T>(where).Get, @default) {}

		public One(Func<ReadOnlyMemory<T>, ReadOnlyMemory<T>> where, Func<T> @default)
		{
			_where = @where;
			_default = @default;
		}

		public T Get(ReadOnlyMemory<T> parameter)
		{
			var enumerable = _where(parameter);
			var result     = enumerable.Length == 1 ? enumerable.Span[0] : _default();
			return result;
		}
	}

	/*public interface ISequenceAlteration<T> : IAlteration<IEnumerable<T>> {}*/

	public class WhereSelector<T> : ISelectSequence<T>
	{
		readonly Func<T, bool> _where;

		public WhereSelector(Func<T, bool> where) => _where = where;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Where(_where);
	}

	public interface ISelectSequence<T> : IAlteration<IEnumerable<T>> {}

	/*public class SelectSequence<T> : ISelectSequence<T>
	{
		readonly ReadOnlyMemory<IAlteration<IEnumerable<T>>> _alterations;

		public SelectSequence(params IAlteration<IEnumerable<T>>[] alterations) => _alterations = alterations;

		public IEnumerable<T> Get(IEnumerable<T> parameter)
			=> _alterations.AsEnumerable()
			               .Aggregate(parameter, (enumerable, alteration) => alteration.Get(enumerable));
	}*/
}
