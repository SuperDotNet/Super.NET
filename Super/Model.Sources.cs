using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Reactive;
using IAny = Super.Model.Specifications.IAny;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISource<T> AsSource<T>(this ISource<T> @this) => @this;

		public static ISource<T> Default<T>(this ISource<T> _) => Model.Sources.Default<T>.Instance;

		public static ISelect<TParameter, TResult> Out<TParameter, TResult>(this ISource<TResult> @this, I<TParameter> _)
			=> new DelegatedResult<TParameter, TResult>(@this.Get);

		public static IAny Out<T>(this ISource<T> @this, ISpecification<T> specification) => @this.Out(specification.IsSatisfiedBy);

		public static IAny Out<T>(this ISource<T> @this, ISelect<T, bool> specification) => @this.Out(specification.Get);

		public static ISelect<TFrom, TTo> Out<TFrom, TTo>(this ISource<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(I<TFrom>.Default).Unless(select);

		public static IAny Out<T>(this ISource<T> @this, Func<T, bool> specification)
			=> new DelegatedResultSpecification(new DelegatedSelection<T, bool>(specification, @this.ToDelegate()).Get).Any();

		public static Model.Commands.IAny Out<T>(this ISource<T> @this, ISelect<T, Unit> select)
			=> @this.Out(select.Out());

		public static Model.Commands.IAny Out<T>(this ISource<T> @this, ICommand<T> select)
			=> new DelegatedParameterCommand<T>(select.Execute, @this.Get).Any();

		public static ISource<T> Unless<T>(this ISource<T> @this, ISource<T> then) => @this.Unless(IsAssigned<T>.Default, then);

		public static ISource<T> Unless<T>(this ISource<T> @this, ISpecification<T> specification)
			=> @this.Default().Unless(specification, @this);

		public static ISource<T> Unless<T>(this ISource<T> @this, ISpecification<T> specification, ISource<T> then)
			=> new ValidatedSource<T>(specification, then, @this);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, I<TTo> _)
			=> @this.Select(Activations<TFrom, TTo>.Default);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.Get);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> new DelegatedSelection<TFrom, TTo>(select, @this.Get);

		public static ISource<T> ToSource<T>(this T @this) => Sources<T>.Default.Get(@this);

		public static ISource<T> ToSource<T>(this Func<T> @this) => I<DelegatedSource<T>>.Default.From(@this);

		public static Func<T> ToDelegate<T>(this ISource<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this ISource<T> @this) => Model.Sources.Delegates<T>.Default.Get(@this);
	}
}