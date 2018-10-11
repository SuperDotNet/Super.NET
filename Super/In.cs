using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;

namespace Super
{
	public static class Start
	{
		public static ISource<T> New<T>(Func<T> select) => new DelegatedSource<T>(select);
		public static ISource<T> New<T>(ISpecification<T> specification) => new Source<T>(default).Unless(specification);

		public static IStructure<T, T> Structure<T>() where T : struct => Model.Selection.Structure.Self<T>.Default;

		
	}


	public static class In<T>
	{
		public static ISpecification<T> Is(Func<T, bool> specification) => new DelegatedSpecification<T>(specification);

		public static ICommand<T> Then(Action<T> action) => new DelegatedCommand<T>(action);

		public static ISelect<T, TResult> Cast<TResult>() => Runtime.Objects.Cast<T, TResult>.Default;

		public static ISelect<T, TResult> Cast<TFrom, TResult>(ISelect<TFrom, TResult> @this) => Cast(@this.ToDelegate());

		public static ISelect<T, TResult> Cast<TFrom, TResult>(Func<TFrom, TResult> @this) => Runtime.Objects.Cast<T, TFrom>.Default.Select(@this);

		public static ISelect<T, TResult> CastForValue<TResult>() => ValueAwareCast<T, TResult>.Default;

		public static ISelect<T, TResult> New<TResult>() => Select(Activator<TResult>.Default);

		public static ISelect<T, TResult> Activate<TResult>() where TResult : IActivateMarker<T> => MarkedActivations<T, TResult>.Default;

		public static ISelect<T, TResult> Default<TResult>() => Select(Model.Sources.Default<TResult>.Instance);

		public static ISelect<T, TResult> Start<TResult>(TResult @this) => new FixedResult<T, TResult>(@this);

		public static ISelect<T, T> Start() => Model.Selection.Self<T>.Default;

		public static ISelect<T, TResult> Select<TResult>(ISource<TResult> @this) => @this.Out(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);

		public static ISelect<T, Type> Type() => InstanceTypeSelector<T>.Default;
	}
}