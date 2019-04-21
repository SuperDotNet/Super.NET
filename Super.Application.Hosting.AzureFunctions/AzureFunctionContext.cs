using Microsoft.AspNetCore.Mvc;
using Super.Model.Selection;

namespace Super.Application.Hosting.AzureFunctions
{
	sealed class AzureFunctionContext<T> : ApplicationContext<AzureFunctionParameter, IActionResult>
		where T : class, ISelect<AzureFunctionParameter, IActionResult>
	{
		public AzureFunctionContext(T application, IServices services) : base(application, services.ToDisposable()) {}
	}
}