using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using System.Threading;

namespace Super.Runtime.Execution
{
	public interface ICounter : IResult<int>, ICommand {}

	public static class Extensions
	{
		public static int Count(this ICounter @this) => @this.Parameter().Get();
	}

	public sealed class Counter : ICounter
	{
		int _count;

		public int Get() => _count;

		public void Execute(None parameter)
		{
			Interlocked.Increment(ref _count);
		}
	}

	sealed class Counter<T> : Select<T, int>
	{
		public Counter() : base(Start.A.Selection<T>()
		                             .AndOf<Counter>()
		                             .By.Instantiation.ToTable()
		                             .Select(x => x.Count())) {}
	}

	sealed class First<T> : DecoratedCondition<T>
	{
		public First() : base(Start.A.Selection<T>()
		                           .AndOf<First>()
		                           .By.Activation()
		                           .ToTable()
		                           .Select(ConditionSelector.Default)) {}
	}

	public sealed class First : ICondition
	{
		readonly ICounter _counter;

		[UsedImplicitly]
		public First() : this(new Counter()) {}

		public First(ICounter counter) => _counter = counter;

		public bool Get(None parameter) => _counter.Get() == 0 && _counter.Count() == 1;
	}
}