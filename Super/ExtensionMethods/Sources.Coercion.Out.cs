namespace Super.ExtensionMethods
{
	partial class Model
	{
		/*public static ISource<TParameter, TTo> Account<TParameter, TTo>(this ISource<TParameter, object> @this,
		                                                                Model.Containers.Cast<TTo> _)
			=> /*@this.SelectOut(_)
			        .Or(@this.SelectOut(Model.Containers.Cast<IInstance<TTo>>.Default)
			                 .SelectOut(InstanceValueCoercer<TTo>.Default.Assigned()))#1#
		@this.SelectOut(AccountForContainer<TTo>.Default);*/

		/*public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     Cast<TTo> _)
			=> @this.Out(_, Default<TResult, TTo>.Instance);

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(
			this ISource<TParameter, TResult> @this, Cast<TTo> _, ISource<TResult, TTo> fallback)
			=> @this.Out(CanCast<TResult, TTo>.Default.If(CastCoercer<TResult, TTo>.Default, fallback));*/

		/*public static ISource<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISource<TParameter, TFrom> @this, I<TTo> _)
			where TTo : IActivateMarker<TFrom> => @this.Out(Activations<TFrom, TTo>.Default);*/

		/*public static ISource<TParameter, TResult> Invoke<TParameter, TResult>(
			this ISource<TParameter, Func<TResult>> @this) => @this.Out(InvokeCoercer<TResult>.Default);*/

		/*public static ISource<TParameter, TTo> Assigned<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                          ISource<TResult, TTo> coercer)
			=> @this.Out(IsAssigned<TResult>.Default.If(coercer));*/

		/*public static ISource<TParameter, TResult> Reduce<TParameter, TInput, TResult>(
			this ISource<TParameter, ISource<TInput, TResult>> @this, Func<TInput> seed)
			=> @this.Out(new DelegatedParameterCoercer<TInput, TResult>(seed));*/

		/*public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     ISource<TResult, TTo> coercer)
			=> @this.Out(coercer.ToDelegate());

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     Func<TResult, TTo> coercer)
			=> @this.ToDelegate().Out(coercer);

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this Func<TParameter, TResult> @this,
		                                                                     Func<TResult, TTo> coercer)
			=> new SelectedResult<TParameter, TResult, TTo>(@this, coercer);*/

		/**/



		/*public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> @this.Out(specification, Defaults<TParameter, TResult>.Default.Get(@this));

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> fallback)
			=> Out(@this, IsAssigned<TResult>.Default, fallback);*/
	}
}