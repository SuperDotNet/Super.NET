using Microsoft.AspNetCore.Mvc;
using Super.Model.Selection;

namespace Super.Application.Hosting.AzureFunctions
{
	public interface IAzureFunction : ISelect<AzureFunctionParameter, IActionResult> {}
}