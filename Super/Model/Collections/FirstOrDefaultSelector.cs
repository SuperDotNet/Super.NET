using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class Assigned<T> : Where<T> where T : class
	{
		public static Assigned<T> Default { get; } = new Assigned<T>();

		Assigned() : base(x => x != null) {}
	}

	public sealed class AssignedValue<T> : WhereSelector<T?> where T : struct
	{
		public static AssignedValue<T> Default { get; } = new AssignedValue<T>();

		AssignedValue() : base(x => x != null) {}
	}

	public sealed class FirstOrDefault<T> : ISelect<ReadOnlyMemory<T>, T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() {}

		public T Get(ReadOnlyMemory<T> parameter) => parameter.Length > 0 ? parameter.Span[0] : default;
	}

	public sealed class FirstOrDefaultSelector<T> : Select<IEnumerable<T>, T>
	{
		public static FirstOrDefaultSelector<T> Default { get; } = new FirstOrDefaultSelector<T>();

		FirstOrDefaultSelector() : base(enumerable => enumerable.FirstOrDefault()) {}
	}

	public sealed class SingleSelector<T> : Select<IEnumerable<T>, T>
	{
		public static SingleSelector<T> Default { get; } = new SingleSelector<T>();

		SingleSelector() : base(x => x.Single()) {}
	}
}