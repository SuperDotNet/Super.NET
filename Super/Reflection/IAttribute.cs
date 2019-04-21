using Super.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public interface IAttribute<out T> : IConditional<ICustomAttributeProvider, T> where T : Attribute {}
}