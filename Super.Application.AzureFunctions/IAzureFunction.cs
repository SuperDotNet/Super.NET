using Microsoft.AspNetCore.Mvc;
using Super.Model.Sources;

namespace Super.Application.AzureFunctions
{
	public interface IAzureFunction : ISource<AzureFunctionParameter, IActionResult> {}
}