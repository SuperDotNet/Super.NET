using Super.Compose;
using Super.Model.Selection.Alterations;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Application
{
	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.In(type)),
			       GenericTypeDefinition.Default.If(IsDefinedGenericType.Default)) {}
	}
}