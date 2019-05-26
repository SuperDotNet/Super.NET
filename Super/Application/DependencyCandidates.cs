using System;
using Super.Compose;
using Super.Model.Sequences;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime.Activation;

namespace Super.Application
{
	sealed class DependencyCandidates : ArrayStore<Type, Type>, IActivateUsing<Type>
	{
		public DependencyCandidates(Type type) : base(A.This(TypeMetadata.Default)
		                                               .Select(Constructors.Default)
		                                               .Query()
		                                               .SelectMany(Parameters.Default.Open())
		                                               .Select(ParameterType.Default)
		                                               .Select(new GenericTypeDependencySelector(type))
		                                               .Where(IsClass.Default)
		                                               .Out()) {}
	}
}