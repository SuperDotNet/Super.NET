using Microsoft.AspNetCore.Mvc;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Application.AzureFunctions
{
	sealed class ApplicationContexts<T> :
		ApplicationContexts<AzureFunctionContext<T>, AzureFunctionParameter, IActionResult>,
		IApplicationContexts where T : class, ISource<AzureFunctionParameter, IActionResult>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Activate.New<AzureFunctionParameter, AzureFunctionArgument>()
		                                     .Out(Services<AzureFunctionArgument>.Default)) {}
	}
}