using Microsoft.AspNetCore.Mvc;

namespace Super.Application.AzureFunctions
{
	public interface IApplicationContexts
		: IApplicationContexts<AzureFunctionParameter, IApplicationContext<AzureFunctionParameter, IActionResult>> {}
}