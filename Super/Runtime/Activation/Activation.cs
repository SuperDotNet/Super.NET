using Super.Model.Selection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using Instances = Super.Runtime.Invocation.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class Activation<TParameter, TResult> : Select<TParameter, TResult>
	{
		public static ISelect<TParameter, TResult> Default { get; } = new Activation<TParameter, TResult>();

		Activation() : base(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default)
		                    .Out(ParameterConstructors<TParameter, TResult>.Default.Assigned())
		                    .Or(new ParameterConstructors<TParameter, TResult>(Instances.Default)
			                        .In(ConstructorLocator.Default))
		                    .Get(Type<TResult>.Metadata)) {}
	}

	public sealed class Activation<T> : FixedActivator<T>
	{
		public static Activation<T> Default { get; } = new Activation<T>();

		Activation() : base(Constructors<T>.Default
		                                   .In(ConstructorLocator.Default)
		                                   .In(TypeMetadataSelector.Default)
		                                   .Out(Call<T>.Default)) {}
	}
}