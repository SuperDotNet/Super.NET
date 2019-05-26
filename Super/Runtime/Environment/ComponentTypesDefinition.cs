using System;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypesDefinition : Model.Results.Result<ISelect<Type, Array<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default.Query()
		                                       .Where(CanActivate.Default.Get)
		                                       .To(x => x.Get().Then())
		                                       .Activate<ComponentTypesSelector>()
		                                       .Select(x => x.Open().Then().Sort().Out())
		                                       .Selector()) {}

		public ComponentTypesDefinition(Func<ISelect<Type, Array<Type>>> source) : base(source) {}
	}
}