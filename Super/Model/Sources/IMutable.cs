using Super.Model.Commands;
using Super.Model.Specifications;
using Super.Runtime;
using Super.Runtime.Activation;
using System.Reactive;

namespace Super.Model.Sources
{
	public interface IMutable<T> : ISource<T>, ICommand<T> {}

	public class Variable<T> : IMutable<T>
	{
		T _instance;

		public Variable() : this(default) {}

		public Variable(T instance) => _instance = instance;

		public T Get() => _instance;

		public void Execute(T parameter)
		{
			_instance = parameter;
		}
	}

	public class DecoratedMutable<T> : IMutable<T>
	{
		readonly IMutable<T> _mutable;

		public DecoratedMutable(IMutable<T> mutable) => _mutable = mutable;

		public T Get() => _mutable.Get();

		public void Execute(T parameter)
		{
			_mutable.Execute(parameter);
		}
	}

	public interface IStore<T> : IMutable<T>, ISpecification {}

	public class Store<T> : DecoratedMutable<T>, IStore<T>, IActivateMarker<IMutable<T>>
	{
		readonly ISpecification<Unit> _specification;

		public Store(IMutable<T> mutable) : this(mutable.Out(IsAssigned<T>.Default), mutable) {}

		public Store(ISpecification<Unit> specification, IMutable<T> mutable) : base(mutable)
			=> _specification = specification;

		public bool IsSatisfiedBy(Unit parameter) => _specification.IsSatisfiedBy(parameter);
	}
}