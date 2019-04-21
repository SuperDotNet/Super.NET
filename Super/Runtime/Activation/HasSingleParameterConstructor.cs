using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DelegatedCondition<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor() : this(Parameters.Default) {}

		public HasSingleParameterConstructor(ISelect<ConstructorInfo, Array<ParameterInfo>> parameters)
			: base(parameters.Query()
			                 .FirstAssigned()
			                 .Select(A.This(ParameterType.Default)
			                          .Then()
			                          .Metadata()
			                          .Select(IsAssignableFrom<T>.Default)
			                          .Assigned()
			                          .Get())
			                 .Then()
			                 .And(parameters.Then().Select(RemainingParametersAreOptional.Default))) {}
	}
}