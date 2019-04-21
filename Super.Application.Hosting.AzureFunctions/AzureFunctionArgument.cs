using Super.Runtime.Activation;

namespace Super.Application.Hosting.AzureFunctions
{
	public sealed class AzureFunctionArgument : ApplicationArgument<AzureFunctionParameter>,
	                                            IActivateUsing<AzureFunctionParameter>
	{
		public AzureFunctionArgument(AzureFunctionParameter instance) : base(instance) {}
	}
}