using Super.Model.Results;
using Super.Model.Selection.Conditions;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class IsDeployed : DelegatedResultCondition
	{
		public static IsDeployed Default { get; } = new IsDeployed();

		IsDeployed() : this(IsAssemblyDeployed.Default, PrimaryAssembly.Default) {}

		public IsDeployed(ICondition<Assembly> condition, IResult<Assembly> result) 
			: base(condition.In(result).Singleton().Get) {}
	}
}