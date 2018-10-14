using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Sequences.Query
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
		readonly Func<Array<T>, Array<T>> _where;
		readonly Func<T> _default;

		public One(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public One(Func<T, bool> where, Func<T> @default) : this(new Where<T>(where).Get, @default) {}

		public One(Func<Array<T>, Array<T>> where, Func<T> @default)
		{
			_where = @where;
			_default = @default;
		}

		public T Get(Array<T> parameter)
		{
			var enumerable = _where(parameter);
			var result     = enumerable.Length == 1 ? enumerable[0] : _default();
			return result;
		}
	}
}
