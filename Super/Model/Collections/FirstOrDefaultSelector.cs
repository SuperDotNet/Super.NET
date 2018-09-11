using Super.Model.Selection;
using Super.Model.Specifications;
using System;

namespace Super.Model.Collections
{
	public sealed class Assigned<T> : Where<T> where T : class
	{
		public static Assigned<T> Default { get; } = new Assigned<T>();

		Assigned() : base(x => x != null) {}
	}

	/*public sealed class AssignedValue<T> : WhereSelector<T?> where T : struct
	{
		public static AssignedValue<T> Default { get; } = new AssignedValue<T>();

		AssignedValue() : base(x => x != null) {}
	}*/

	public interface IReduce<T> : ISelect<ReadOnlyMemory<T>, T> {}

	public sealed class FirstOrDefault<T> : FirstWhere<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(Always<T>.Default) {}
	}

	public sealed class FirstAssigned<T> : FirstWhere<T> where T : class
	{
		public static FirstAssigned<T> Default { get; } = new FirstAssigned<T>();

		FirstAssigned() : base(x => x != null) {}
	}

	public sealed class FirstAssignedValue<T> : FirstWhere<T?> where T : struct
	{
		public static FirstAssignedValue<T> Default { get; } = new FirstAssignedValue<T>();

		FirstAssignedValue() : base(x => x != null) {}
	}

	public class FirstWhere<T> : IReduce<T>
	{
		/*public static FirstWhere<T> Default { get; } = new FirstWhere<T>();

		FirstWhere() : this(Always<T>.Default) {}*/

		readonly Func<T, bool> _where;
		readonly Func<T> _default;

		public FirstWhere(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public FirstWhere(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where = @where;
			_default = @default;
		}

		public T Get(ReadOnlyMemory<T> parameter)
		{
			var length = parameter.Length;
			var span   = parameter.Span;

			for (var i = 0; i < length; i++)
			{
				var item = span[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default.Invoke();
		}
	}

	/*public sealed class FirstOrDefaultSelector<T> : Select<IEnumerable<T>, T>
	{
		public static FirstOrDefaultSelector<T> Default { get; } = new FirstOrDefaultSelector<T>();

		FirstOrDefaultSelector() : base(enumerable => enumerable.FirstOrDefault()) {}
	}*/

	/*public sealed class SingleSelector<T> : Select<IEnumerable<T>, T>
	{
		public static SingleSelector<T> Default { get; } = new SingleSelector<T>();

		SingleSelector() : base(x => x.Single()) {}
	}*/
}