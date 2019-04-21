using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public interface IAttributes<T> : IConditional<ICustomAttributeProvider, Array<T>> where T : Attribute {}
}