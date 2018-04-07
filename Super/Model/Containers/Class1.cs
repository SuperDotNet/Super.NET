using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Containers
{
	public interface ISelect<T>
	{
		Func<TOut, T> Out<TOut>();

		Func<T, TIn> In<TIn>();
	}

	public sealed class New<T> : ISelect<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() {}

		public Func<TOut, T> Out<TOut>() => Instances<TOut, T>.Default.ToDelegate();

		public Func<T, TIn> In<TIn>() => Instances<T, TIn>.Default.ToDelegate();
	}

	sealed class Container<T> : ISelect<T>
	{
		public static Container<T> Default { get; } = new Container<T>();

		Container() {}

		public Func<TOut, T> Out<TOut>() => Delegate<TOut, T>.Instance;

		public Func<T, TIn> In<TIn>() => Delegate<T, TIn>.Instance;

		sealed class Delegate<TFrom, TTo> : DecoratedSource<TFrom, TTo>
		{
			public static Func<TFrom, TTo> Instance { get; } = new Delegate<TFrom, TTo>().ToDelegate();

			Delegate()
				: base(Cast<T>.Delegate<TFrom, TTo>.Instance.ToSource()
				              .Or(Cast<T>.Delegate<TFrom, IInstance<TTo>>.Instance.ToSource()
				                         .Out(InstanceValueCoercer<TTo>.Default.Assigned()))) {}
		}
	}

	public sealed class Cast<T> : ISelect<T>
	{
		public static ISelect<T> Default { get; } = new Cast<T>();

		Cast() {}

		public Func<TFrom, T> Out<TFrom>() => Delegate<TFrom, T>.Instance;

		public Func<T, TTo> In<TTo>() => Delegate<T, TTo>.Instance;

		public sealed class Delegate<TFrom, TTo> : DecoratedSource<TFrom, TTo>
		{
			public static Func<TFrom, TTo> Instance { get; } = new Delegate<TFrom, TTo>().ToDelegate();

			Delegate() : base(CanCast<TFrom, TTo>.Default.If(CastCoercer<TFrom, TTo>.Default, Default<TFrom, TTo>.Instance)) {}
		}
	}
}