using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Reactive;
using System.Threading;

namespace Super.Runtime.Execution
{
	static class Ambient
	{
		/*public static ISource<T> For<T>() where T : class => Activator<T>.Default.To(AmbientAlteration<T>.Default);*/

		public static ISource<T> ToAmbient<T>(this ISource<T> @this) => @this.To(AmbientAlteration<T>.Default);
	}

	public interface ICounter : ISource<int>, ICommand {}

	public static class Extensions
	{
		public static int Count(this ICounter @this) => @this.Executed().Get();
	}

	sealed class Counter : ICounter
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
		public Counter() : base(In<T>.New<Counter>().ToTable().Out(x => x.Count())) {}
	}

	sealed class First<T> : DecoratedSpecification<T>
	{
		public First() : base(In<T>.New<First>().ToTable().Out(ConditionSelector.Default).ToSpecification()) {}
	}

	public sealed class First : ISpecification
	{
		readonly ICounter _counter;

		[UsedImplicitly]
		public First() : this(new Counter()) {}

		public First(ICounter counter) => _counter = counter;

		public bool IsSatisfiedBy(Unit parameter) => _counter.Get() == 0 && _counter.Count() == 1;
	}

	public class Ambient<T> : Deferred<T>, IActivateMarker<ISource<T>>
	{
		public Ambient(Func<T> factory) : this(new DelegatedSource<T>(factory)) {}

		public Ambient(ISource<T> source) : base(source, new Logical<T>()) {}
	}
}