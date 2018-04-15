using Super.Model.Selection;
using System;

namespace Super.Reflection.Types
{
	public interface ITyped<out T> : ISelect<Type, T> {}
}