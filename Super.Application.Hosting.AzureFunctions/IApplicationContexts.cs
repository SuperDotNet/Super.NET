using Microsoft.AspNetCore.Mvc;

namespace Super.Application.Hosting.AzureFunctions
{
	public interface IApplicationContexts
		: IApplicationContexts<AzureFunctionParameter, IApplicationContext<AzureFunctionParameter, IActionResult>> {}
}