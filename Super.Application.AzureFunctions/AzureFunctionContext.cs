using Microsoft.AspNetCore.Mvc;
using Super.Model.Sources;

namespace Super.Application.AzureFunctions
{
	sealed class AzureFunctionContext<T> : ApplicationContext<AzureFunctionParameter, IActionResult>
		where T : class, ISource<AzureFunctionParameter, IActionResult>
	{
		public AzureFunctionContext(T application, IServices services) : base(application, services) {}
	}
}