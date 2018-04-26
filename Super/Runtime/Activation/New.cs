﻿using Super.Model.Selection;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System;
using Instances = Super.Runtime.Invocation.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class New<TParameter, TResult> : Select<TParameter, TResult>
	{
		public static ISelect<TParameter, TResult> Default { get; } = new New<TParameter, TResult>();

		New() : base(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default)
		             .Out(ParameterConstructors<TParameter, TResult>.Default.Assigned())
		             .Or(ConstructorLocator.Default.Out(new ParameterConstructors<TParameter, TResult>(Instances.Default)))
		             .Get(Type<TResult>.Metadata)) {}
	}

	public sealed class New<T> : FixedActivator<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() : base(In<Type>.Start(x => x.Metadata())
		                     .Out(ConstructorLocator.Default)
		                     .Out(Constructors<T>.Default)
		                     .Invoke()) {}
	}
}