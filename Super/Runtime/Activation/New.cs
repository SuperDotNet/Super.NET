using Super.Model.Selection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System;
using Instances = Super.Runtime.Invocation.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class New<TParameter, TResult> : Select<TParameter, TResult>
	{
		public static ISelect<TParameter, TResult> Default { get; } = new New<TParameter, TResult>();

		New() : base(ConstructorLocator.Default
		                               .Select(new ParameterConstructors<TParameter, TResult>(Instances.Default))
		                               .Unless(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default)
			                                       .Select(ParameterConstructors<TParameter, TResult>.Default.Assigned()))
		                               .Get(Type<TResult>.Metadata)) {}
	}

	public sealed class New<T> : FixedActivator<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() : base(Self<Type>.Default.Metadata()
		                       .Select(ConstructorLocator.Default)
		                       .Select(Constructors<T>.Default)
		                       .Invoke()) {}
	}
}