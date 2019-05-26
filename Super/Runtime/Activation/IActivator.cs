using System;
using Super.Model.Results;
using Super.Model.Selection;

namespace Super.Runtime.Activation
{
	public interface IActivator<out T> : IResult<T> {}

	public interface IActivator : ISelect<Type, object> {}
}