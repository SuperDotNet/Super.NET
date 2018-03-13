using System;
using Super.Model.Instances;
using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	public interface IActivator<out T> : IInstance<T> {}

	public interface IActivator : ISource<Type, object> {}
}