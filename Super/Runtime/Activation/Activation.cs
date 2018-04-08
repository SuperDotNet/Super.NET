using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime.Invocation;
using System;
using Super.Model.Selection;
using Super.Model.Sources;
using Instances = Super.Runtime.Invocation.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class Activation<TParameter, TResult> : Delegated<TParameter, TResult>
	{
		public static ISelect<TParameter, TResult> Default { get; } = new Activation<TParameter, TResult>();

		Activation() : base(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default)
		                    .Out(ParameterConstructors<TParameter, TResult>
		                         .Default
		                         .Assigned())
		                    .Or(new ParameterConstructors<TParameter, TResult>(Instances.Default)
			                        .In(ConstructorLocator.Default))
		                    .Get(Types<TResult>.Key)) {}
	}

	public sealed class Activation<T> : FixedSelection<Type, T>, IActivator<T>
	{
		public static Activation<T> Default { get; } = new Activation<T>();

		Activation() : base(Constructors<T>.Default
		                                   .In(ConstructorLocator.Default)
		                                   .In(TypeMetadataSelector.Default)
		                                   .Out(Invoke<T>.Default), Types<T>.Identity) {}
	}
}