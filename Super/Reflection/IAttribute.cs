using System;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection
{
	public interface IAttribute<out T> : IConditional<ICustomAttributeProvider, T> where T : Attribute {}
}