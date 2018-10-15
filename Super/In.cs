using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super
{
	public static class Start
	{
		public static ISpecification<T> Assigned<T>() => IsAssigned<T>.Default;

		public static ISpecification<T> When<T>(Func<T, bool> specification)
			=> new DelegatedSpecification<T>(specification);

		/**/

		public static ISelect<T, T> With<T>() => Self<T>.Default;

		public static ISelect<object, Type> Type() => InstanceType<object>.Default;

		public static ISelect<T, Type> Type<T>() => InstanceType<T>.Default;

		public static ISelect<object, TypeInfo> Metadata() => InstanceMetadata<object>.Default;

		public static ISelect<T, TypeInfo> Metadata<T>() => InstanceMetadata<T>.Default;

		/**/

		public static ISource<T> Default<T>() => Model.Sources.Default<T>.Instance;

		public static ISource<T> With<T>(T instance) => new Source<T>(instance);

		public static ISource<T> With<T>(Func<T> select) => new DelegatedSource<T>(select);
	}

	public static class In<T>
	{
		public static ISelect<T, TResult> New<TResult>() => Select(Activator<TResult>.Default);

		public static ISelect<T, TResult> Activate<TResult>() where TResult : IActivateMarker<T>
			=> MarkedActivations<T, TResult>.Default;

		public static ISelect<T, TResult> Default<TResult>() => Select(Model.Sources.Default<TResult>.Instance);

		public static ISelect<T, TResult> Start<TResult>(TResult @this) => new FixedResult<T, TResult>(@this);

		public static ISelect<T, T> Start() => Self<T>.Default;

		public static ISelect<T, TResult> Select<TResult>(ISource<TResult> @this) => @this.Out(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);


	}
}