using Super.Model.Commands;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Reactive;

// ReSharper disable TooManyArguments

namespace Super.ExtensionMethods
{
	public static class Commands
	{
		public static ICommand<Unit> Select<T>(this ICommand<T> @this, T parameter) => Select(@this, new FixedResult<Unit, T>(parameter));

		public static ICommand<TFrom> Select<TFrom, TTo>(this ICommand<TTo> @this, ISource<TFrom, TTo> select)
			=> Select(@this, @select.ToDelegate());

		public static ICommand<TFrom> Select<TFrom, TTo>(this ICommand<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterCommand<TFrom,TTo>(@this.ToDelegate(), select);

		public static void Execute<T>(this ICommand<ImmutableArray<T>> @this, params T[] parameters) =>
			@this.Execute(parameters.ToImmutableArray());

		public static void Execute<T1, T2>(this ICommand<(T1, T2)> @this, T1 first, T2 second) =>
			@this.Execute(ValueTuple.Create(first, second));

		public static void Execute<T1, T2, T3>(this ICommand<(T1, T2, T3)> @this, T1 first, T2 second, T3 third) =>
			@this.Execute(ValueTuple.Create(first, second, third));

		public static Action<T> ToDelegate<T>(this ICommand<T> @this) => Super.Model.Commands.Delegates<T>.Default.Get(@this);

		/*public static ISource<T, Unit> Adapt<T>(this ICommand<T> @this) => Super.Model.Commands.Adapters<T>.Default.Get(@this);*/

		/*public static ISource<TParameter, ISource<T, Unit>> Adapt<TParameter, T>(this ISource<TParameter, ICommand<T>> @this)
			=> @this.SelectOut(CommandSourceCoercer<T>.Default);*/

		public static ICommand<T> ToCommand<T>(this ISource<T, Unit> @this) => @this.ToDelegate().ToCommand();

		public static ICommand<T> ToCommand<T>(this Func<T, Unit> @this) => InvokeParameterCommands<T>.Default.Get(@this);

		public static ICommand<Unit> ToCommand<T>(this Func<T> @this) => InvokeCommands<T>.Default.Get(@this);

		public static ICommand<T> ToCommand<T>(this Action<T> @this) => DelegateCommands<T>.Default.Get(@this);


		public static void Assign<TKey, TValue>(this IAssignable<TKey, TValue> @this, TKey key, TValue value)
			=> @this.Executed(Pairs.Create(key, value)).Return(@this);

		public static ICommand<T> Executed<T>(this ICommand<T> @this, T parameter) => @this.Execute(parameter, @this);

		static TReturn Execute<T, TReturn>(this ICommand<T> @this, T parameter, TReturn @return)
		{
			@this.Execute(parameter);
			return @return;
		}

		public static T ReturnWith<TCommand, T>(this TCommand @this, T parameter) where TCommand : class, ICommand<T>
			=> @this.Execute(parameter, parameter);

		public static ICommand<Unit> Executed(this ICommand<Unit> @this) => @this.Executed(Unit.Default);

		public static void Execute(this ICommand<Unit> @this)
		{
			@this.Execute(Unit.Default);
		}

		public static ICommand<T> ToCommand<T>(this IInstance<ICommand<T>> @this)
			=> Activations<IInstance<ICommand<T>>, DelegatedInstanceCommand<T>>.Default.Get(@this);

		public static IAlteration<T> ToConfiguration<T>(this ICommand<T> @this)
			=> @this.To(I<ConfiguringAlteration<T>>.Default);

		public static ISource<T, Unit> Out<T>(this ICommand<T> @this) => @this.ToConfiguration().Out(x => Unit.Default);
	}
}