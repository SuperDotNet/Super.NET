using System;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public interface ITyped<out T> : ISelect<Type, T> {}
}