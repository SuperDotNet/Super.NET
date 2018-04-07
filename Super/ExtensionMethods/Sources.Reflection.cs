namespace Super.ExtensionMethods
{
	partial class Model
	{
		/*public static TResult Get<T, TResult>(this ISource<ImmutableArray<T>, TResult> @this, params T[] parameters) =>
			@this.Get(parameters.ToImmutableArray());*/

		/*public static T Get<T>(this ISource<ImmutableArray<Type>, T> @this, params TypeInfo[] parameters) =>
			@this.Get(parameters.Select(x => x.AsType()).ToImmutableArray());*/

		/*public static T Get<T>(this ISource<Type, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISource<TypeInfo, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISource<Type, T> @this, I<T> parameter) => @this.Get(parameter.Get());

		public static T Get<T>(this ISource<TypeInfo, T> @this, I<T> parameter) => @this.Get(parameter.Get());*/

		/*public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                   ISpecification<Type> specification)
			=> @this.In(specification.Adapt().In(I<TParameter>.Default).ToSpecification(),
			            Defaults<TParameter, TResult>.Default.Get(@this));

		public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                   ISpecification<TypeInfo> specification)
			=> @this.In(specification.Adapt().In(I<TParameter>.Default).ToSpecification(),
			            Defaults<TParameter, TResult>.Default.Get(@this));*/

/*		public static ISource<TParameter, TResult> Out<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                    ISpecification<TypeInfo> specification)
			=> @this.Out(specification.Select(TypeMetadataCoercer.Default));*/

/*
		public static ISource<TParameter, TResult> Out<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                    ISpecification<Type> specification)
			=> specification.Select(InstanceTypeCoercer<TParameter>.Default)
			                .If(@this, Defaults<TParameter, TResult>.Default.Get(@this));
*/

		/*public static T Get<T>(this ISource<Type, object> @this, object _) => (T)@this.Get(typeof(T));

		public static T Get<T>(this ISource<Type, object> @this) => (T)@this.Get(typeof(T));*/
	}
}