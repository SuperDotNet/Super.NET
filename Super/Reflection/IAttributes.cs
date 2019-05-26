using System;
using System.Reflection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Reflection
{
	public interface IAttributes<T> : IConditional<ICustomAttributeProvider, Array<T>> where T : Attribute {}
}