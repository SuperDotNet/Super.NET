using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Invocation;
using System;
using Instances = Super.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class Activation<TParameter, TResult> : DelegatedSource<TParameter, TResult>
	{
		public static ISource<TParameter, TResult> Default { get; } = new Activation<TParameter, TResult>();

		Activation() : base(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default)
		                    .Out(ParameterConstructors<TParameter, TResult>
		                         .Default
		                         .Assigned())
		                    .Or(new ParameterConstructors<TParameter, TResult>(Instances.Default)
			                        .In(ConstructorLocator.Default))
		                    .Get(Types<TResult>.Key)) {}
	}

	public sealed class Activation<T> : FixedParameterSource<Type, T>, IActivator<T>
	{
		public static Activation<T> Default { get; } = new Activation<T>();

		Activation() : base(Constructors<T>.Default
		                                   .In(ConstructorLocator.Default)
		                                   .In(TypeMetadataCoercer.Default)
		                                   .Out(Invoke<T>.Default), Types<T>.Identity) {}
	}
}