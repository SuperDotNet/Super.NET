using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Invocation;
using System;
using Instances = Super.Expressions.Instances;

namespace Super.Runtime.Activation
{
	public sealed class New<TParameter, TResult> : DelegatedSource<TParameter, TResult>
	{
		public static ISource<TParameter, TResult> Default { get; } = new New<TParameter, TResult>();

		New() : base(ParameterConstructors<TParameter, TResult>
		             .Default
		             .Assigned(new ConstructorLocator(HasSingleParameterConstructor<TParameter>.Default))
		             .Or(new ParameterConstructors<TParameter, TResult>(Instances.Default)
			                 .In(ConstructorLocator.Default))
		             .Get(Types<TResult>.Key)) {}
	}

	public sealed class New<T> : FixedParameterSource<Type, T>, IActivator<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() : base(Constructors<T>.Default.In(ConstructorLocator.Default)
		                            .In(TypeMetadataCoercer.Default)
		                            .Out(InvokeCoercer<T>.Default), Types<T>.Identity) {}
	}
}