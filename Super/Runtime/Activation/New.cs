﻿using Super.Compose;
using Super.Model.Selection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime.Invocation.Expressions;

namespace Super.Runtime.Activation
{
	public sealed class New<TIn, TOut> : Select<TIn, TOut>
	{
		public static ISelect<TIn, TOut> Default { get; } = new New<TIn, TOut>();

		New() : base(A.This(ConstructorLocator.Default)
		              .Select(new ParameterConstructors<TIn, TOut>(ConstructorExpressions.Default))
		              .Unless(A.This(new ConstructorLocator(HasSingleParameterConstructor<TIn>.Default))
		                       .Select(ParameterConstructors<TIn, TOut>.Default.Assigned()))
		              .Get(Type<TOut>.Metadata)) {}
	}

	public sealed class New<T> : FixedActivator<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() : base(Start.A.Selection(TypeMetadata.Default)
		                  .Select(ConstructorLocator.Default)
		                  .Select(Constructors<T>.Default)
		                  .Then()
		                  .Invoke()
		                  .Get()) {}
	}
}