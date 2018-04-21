using Microsoft.AspNetCore.Mvc;
using Super.Model.Selection;

namespace Super.Application.Host.AzureFunctions
{
	public interface IAzureFunction : ISelect<AzureFunctionParameter, IActionResult> {}
}