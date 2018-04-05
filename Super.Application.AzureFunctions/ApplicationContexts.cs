using Microsoft.AspNetCore.Mvc;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Application.AzureFunctions
{
	sealed class ApplicationContexts<T> :
		ApplicationContexts<AzureFunctionContext<T>, AzureFunctionParameter, IActionResult>,
		IApplicationContexts where T : class, ISource<AzureFunctionParameter, IActionResult>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(From.New<AzureFunctionParameter, AzureFunctionArgument>()
		                                 .Out(Services<AzureFunctionArgument>.Default)) {}
	}
}