using Super.Model.Selection;
using System;

namespace Super.Reflection
{
	public interface ITyped<out T> : ISelect<Type, T> {}
}