using System;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Aspects
{
	public interface IRegistration : IConditional<Array<Type>, object> {}
}