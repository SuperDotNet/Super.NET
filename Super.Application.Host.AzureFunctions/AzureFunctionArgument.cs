using Super.Runtime.Activation;

namespace Super.Application.Host.AzureFunctions
{
	public sealed class AzureFunctionArgument : ApplicationArgument<AzureFunctionParameter>,
	                                            IActivateMarker<AzureFunctionParameter>
	{
		public AzureFunctionArgument(AzureFunctionParameter instance) : base(instance) {}
	}
}