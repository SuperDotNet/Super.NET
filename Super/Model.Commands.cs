using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Reactive;

// ReSharper disable TooManyArguments

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ICommand<Unit> Select<T>(this ICommand<T> @this, T parameter) => @this.Select(parameter.ToSelect());

		public static ICommand<TFrom> Select<TFrom, TTo>(this ICommand<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(ToDelegate(@select));

		public static ICommand<TFrom> Select<TFrom, TTo>(this ICommand<TTo> @this, ISelect<TFrom> select)
			=> @this.Select(select.In<TTo>());

		public static ICommand<TFrom> Select<TFrom, TTo>(this ICommand<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterCommand<TFrom, TTo>(@this.ToDelegate(), select);

		public static void Execute<T>(this ICommand<ImmutableArray<T>> @this, params T[] parameters) =>
			@this.Execute(parameters.ToImmutableArray());

		public static void Execute<T1, T2>(this ICommand<(T1, T2)> @this, T1 first, T2 second) =>
			@this.Execute(ValueTuple.Create(first, second));

		public static void Execute<T1, T2, T3>(this ICommand<(T1, T2, T3)> @this, T1 first, T2 second, T3 third) =>
			@this.Execute(ValueTuple.Create(first, second, third));

		public static Action<T> ToDelegate<T>(this ICommand<T> @this) => Model.Commands.Delegates<T>.Default.Get(@this);

		public static ISelect<T, Unit> Out<T>(this ICommand<T> @this) => Out(ToConfiguration(@this), _ => Unit.Default);

		public static ICommand<T> ToCommand<T>(this ISelect<T, Unit> @this) => ToDelegate(@this).ToCommand();

		public static ICommand<T> ToCommand<T>(this Func<T, Unit> @this) => InvokeParameterCommands<T>.Default.Get(@this);

		public static ICommand<Unit> ToCommand<T>(this Func<T> @this) => InvokeCommands<T>.Default.Get(@this);

		public static ICommand<T> ToCommand<T>(this Action<T> @this) => DelegateCommands<T>.Default.Get(@this);

		public static ICommand<T> ToCommand<T>(this ISource<ICommand<T>> @this)
			=> I<DelegatedInstanceCommand<T>>.Default.From(@this);

		public static ICommand<TParameter> ToCommand<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> ToDelegate(@this).ToCommand();

		public static ICommand<TParameter> ToCommand<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> @this.To(I<InvokeParameterCommand<TParameter, TResult>>.Default);

		public static void Assign<TKey, TValue>(this IAssignable<TKey, TValue> @this, TKey key, TValue value)
			=> Executed(@this, Pairs.Create(key, value)).Return(@this);

		public static ICommand<T> Executed<T>(this ICommand<T> @this, T parameter) => @this.Execute(parameter, @this);

		static TReturn Execute<T, TReturn>(this ICommand<T> @this, T parameter, TReturn @return)
		{
			@this.Execute(parameter);
			return @return;
		}

		public static T ReturnWith<TCommand, T>(this TCommand @this, T parameter) where TCommand : class, ICommand<T>
			=> @this.Execute(parameter, parameter);


		public static T Executed<T>(this T @this) where T : class, ICommand => @this.Execute(Unit.Default, @this);

		public static void Execute(this ICommand<Unit> @this)
		{
			@this.Execute(Unit.Default);
		}

		public static IAlteration<T> ToConfiguration<T>(this ICommand<T> @this)
			=> @this.To(I<ConfiguringAlteration<T>>.Default);
	}
}