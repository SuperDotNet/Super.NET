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
}
