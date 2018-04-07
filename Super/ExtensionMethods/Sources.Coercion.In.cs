namespace Super.ExtensionMethods
{
	partial class Model
	{
		/*public static ITable<TTo, TResult> In<TFrom, TTo, TResult>(
			this ITable<TFrom, TResult> @this, ISource<TTo, TFrom> coercer)
			=> new CoercedTable<TFrom, TTo, TResult>(@this, coercer);*/

		/*public static ITable<TParameter, TResult> Guarded<TParameter, TResult>(this ITable<TParameter, TResult> @this)
			=> @this.In(AssignedArgumentGuard<TParameter>.Default);

		public static ITable<TParameter, TResult> Assigned<TParameter, TResult>(this ITable<TParameter, TResult> @this)
			=> @this.In(AssignedSpecification<TParameter>.Default);*/

		/*public static ITable<TParameter, TResult> In<TParameter, TResult>(
			this ITable<TParameter, TResult> @this, ISpecification<TParameter> specification)
			=> new ValidatedTable<TParameter, TResult>(specification, @this);*/

		/*public static ISource<Decoration<TFrom, TResult>, TResult> In<TFrom, TTo, TResult>(
			this ISource<Decoration<TTo, TResult>, TResult> @this, I<TFrom> _)
			=> @this.In(DecorationParameterCoercer<TFrom, TTo, TResult>.Default);*/

		/*static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                            ISpecification<TParameter> specification)
			=> ;*/

		/*static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this, Model.Containers.Cast<TFrom> _)
			=> @this.SelectIn<TFrom, TTo, TResult>(_);*/

		/*public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this, I<TFrom> _)
			where TTo : IActivateMarker<TFrom> => @this.In(Activations<TFrom, TTo>.Default);*/

		/*public static ISource<TFrom, TResult> Assigned<TFrom, TTo, TResult>(
			this ISource<TTo, TResult> @this, ISource<TFrom, TTo> coercer)
		{
			var temp = IsAssigned<TTo>.Default.If(@this);

			/*coercer.SelectOut(IsAssigned<TTo>.Default.If(@this))#1#
			return null;
		}*/

		/*public static ISource<TTo, TResult> In<TFrom, TTo, TResult>(this ISource<TFrom, TResult> @this,
		                                                            ISource<TTo, TFrom> coercer)
			=> @this.In(coercer.ToDelegate());*/

		/*public static ISource<TTo, TResult> In<TFrom, TTo, TResult>(this ISource<TFrom, TResult> @this,
		                                                              Func<TTo, TFrom> coercer)
			=> @this.ToDelegate().In(coercer);*/

		/*public static ISource<TTo, TResult> In<TFrom, TTo, TResult>(this Func<TFrom, TResult> @this,
		                                                            Func<TTo, TFrom> coercer)
			=> new SelectedParameterSource<TFrom, TTo, TResult>(@this, coercer);*/

		/*static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<Type, TResult> @this,
		                                                                   I<TParameter> _ = null)
			=> @this.In(InstanceTypeCoercer<TParameter>.Default);*/

		/*static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TypeInfo, TResult> @this,
		                                                                   I<TParameter> _ = null)
			=> @this.In(InstanceMetadataCoercer<TParameter>.Default);*/
	}
}