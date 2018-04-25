using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Execution;
using System;
using System.Reactive;

namespace Super.Model.Extents
{
	public static class Extensions
	{
		public static IIn<T, bool> In<T>(this ISpecification<T> @this) => new In<T, bool>(@this.IsSatisfiedBy);

		/*public static ISpecification<TTo> In<TFrom, TTo>(this ISpecification<TFrom> @this, ISelect<TTo, TFrom> select)
			=> @this.In(select.ToDelegate());*/

		/*public static ISpecification<TTo> In<TFrom, TTo>(this ISpecification<TFrom> @this, Func<TTo, TFrom> select)
			=> @this.In().In(select).Return();

		public static ISpecification<TOut> In<TIn, TOut>(this ISpecification<TIn> @this,
		                                                 Func<IIn<TIn, bool>, IIn<TOut, bool>> configure)
			=> configure(@this.In()).Return();*/

		public static IIn<T, Unit> In<T>(this ICommand<T> @this) => new Configuration<T>(@this.Execute).In();

		/*public static ICommand<TTo> In<TFrom, TTo>(this ICommand<TFrom> @this, ISelect<TTo, TFrom> select)
			=> @this.In(select.ToDelegate());

		public static ICommand<TTo> In<TFrom, TTo>(this ICommand<TFrom> @this, Func<TTo, TFrom> select)
			=> @this.In().In(select).Return();*/

		public static ICommand<TOut> In<TIn, TOut>(this ICommand<TIn> @this, Func<IIn<TIn, Unit>, IIn<TOut, Unit>> configure)
			=> configure(@this.In()).Return();

		public static IIn<TIn, TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this) => new In<TIn, TOut>(@this.Get);

		public static IOut<TIn, TOut> Out<TIn, TOut>(this ISelect<TIn, TOut> @this) => new Out<TIn, TOut>(@this.Get);

		/*public static ISelect<TTo, TOut> In<TFrom, TTo, TOut>(this ISelect<TFrom, TOut> @this, ISelect<TTo, TFrom> select)
			=> @this.In(select.ToDelegate());

		public static ISelect<TTo, TOut> In<TFrom, TTo, TOut>(this ISelect<TFrom, TOut> @this, Func<TTo, TFrom> select)
			=> @this.In().In(select).Return();*/

		public static ISelect<TTo, TOut> In<TFrom, TTo, TOut>(this ISelect<TFrom, TOut> @this,
		                                                      Func<IIn<TFrom, TOut>, IIn<TTo, TOut>> configure)
			=> configure(@this.In()).Return();

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Out().Out(select).Return();

		public static ISpecification<TIn> Out<TIn, TFrom>(this ISelect<TIn, TFrom> @this, Func<TFrom, bool> select)
			=> @this.Out().Out(select).Return();

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                     Func<IOut<TIn, TFrom>, IOut<TIn, TTo>> configure)
			=> configure(@this.Out()).Return();

		public static ISource<TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                Func<IOut<TIn, TFrom>, IOut<Unit, TTo>> configure)
			=> configure(@this.Out()).Return();

		public static IOut<Unit, T> Out<T>(this ISource<T> @this)
			=> new Out<Unit, T>(new DelegatedResult<Unit, T>(@this.Get).Get);

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Out().Out(select).Return();

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this,
		                                           Func<IOut<Unit, TFrom>, IOut<Unit, TTo>> configure)
			=> configure(@this.Out()).Return();

		public static ISpecification<T> Return<T>(this IIn<T, bool> @this) => new DelegatedSpecification<T>(@this.Get());

		public static ISpecification<T> Return<T>(this IOut<T, bool> @this) => new DelegatedSpecification<T>(@this.Get());

		public static ICommand<T> Return<T>(this IIn<T, Unit> @this) => new InvokeParameterCommand<T>(@this.Get());

		public static ISource<T> Return<T>(this IOut<Unit, T> @this)
			=> new FixedSelection<Unit, T>(@this.ToSelect(), Unit.Default);

		public static ISelect<TIn, TOut> Return<TIn, TOut>(this IIo<TIn, TOut> @this) => @this.ToSelect();

		public static ISelect<TIn, TOut> ToSelect<TIn, TOut>(this IIo<TIn, TOut> @this) => @this.Get().ToSelect();
	}

	public interface IIn<in TIn, out T> : IIo<TIn, T> {}

	sealed class In<TIn, T> : Io<TIn, T>, IIn<TIn, T>
	{
		public In(Func<TIn, T> instance) : base(instance) {}
	}

	public interface IOut<in T, out TOut> : IIo<T, TOut> {}

	sealed class Out<T, TOut> : Io<T, TOut>, IOut<T, TOut>
	{
		public Out(Func<T, TOut> instance) : base(instance) {}
	}

	public interface IIo<in TIn, out TOut> : ISource<Func<TIn, TOut>> {}

	class Io<TIn, TOut> : Source<Func<TIn, TOut>>, IIo<TIn, TOut>
	{
		protected Io(Func<TIn, TOut> instance) : base(instance) {}
	}

	sealed class OnlyOnceAlteration<T> : IAlteration<IIn<T, bool>>
	{
		public static OnlyOnceAlteration<T> Default { get; } = new OnlyOnceAlteration<T>();

		OnlyOnceAlteration() {}

		public IIn<T, bool> Get(IIn<T, bool> parameter) => OnlyOnceAlteration<T, bool>.Default.Get(parameter).And(parameter);
	}

	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<IIn<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public IIn<TIn, TOut> Get(IIn<TIn, TOut> parameter) => parameter.In(new First().In().Allow(I<TIn>.Default).Return());
	}

	sealed class OnceAlteration<T> : IAlteration<IIn<T, bool>>
	{
		public static OnceAlteration<T> Default { get; } = new OnceAlteration<T>();

		OnceAlteration() {}

		public IIn<T, bool> Get(IIn<T, bool> parameter) => OnceAlteration<T, bool>.Default.Get(parameter).And(parameter);
	}

	sealed class OnceAlteration<TIn, TOut> : IAlteration<IIn<TIn, TOut>>
	{
		public static OnceAlteration<TIn, TOut> Default { get; } = new OnceAlteration<TIn, TOut>();

		OnceAlteration() {}

		public IIn<TIn, TOut> Get(IIn<TIn, TOut> parameter) => parameter.In(new First<TIn>());
	}
}