using System;
using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	public interface IActivator<out T> : ISource<T> {}

	public interface IActivator : ISelect<Type, object> {}
}