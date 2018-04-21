using Microsoft.AspNetCore.Mvc;

namespace Super.Application.Host.AzureFunctions
{
	public interface IApplicationContexts
		: IApplicationContexts<AzureFunctionParameter, IApplicationContext<AzureFunctionParameter, IActionResult>> {}
}