using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using System.Reactive;
using System.Threading;

namespace Super.Runtime.Execution
{
	public interface ICounter : ISource<int>, ICommand {}

	public static class Extensions
	{
		public static int Count(this ICounter @this) => @this.Executed().Get();
	}

	public sealed class Counter : ICounter
	{
		int _count;

		public int Get() => _count;

		public void Execute(Unit parameter)
		{
			Interlocked.Increment(ref _count);
		}
	}

	sealed class Counter<T> : DecoratedSelect<T, int>
	{
		public Counter() : base(Start.From<T>().New<Counter>().ToTable().Select(x => x.Count())) {}
	}

	sealed class First<T> : DecoratedSpecification<T>
	{
		public First() : base(Start.From<T>().New<First>().ToTable().Select(ConditionSelector.Default).Out()) {}
	}

	public sealed class First : ISpecification
	{
		readonly ICounter _counter;

		[UsedImplicitly]
		public First() : this(new Counter()) {}

		public First(ICounter counter) => _counter = counter;

		public bool IsSatisfiedBy(Unit parameter) => _counter.Get() == 0 && _counter.Count() == 1;
	}
}