using Microsoft.AspNetCore.Mvc;
using Super.Compose;
using Super.Model.Selection;

namespace Super.Application.Hosting.AzureFunctions
{
	sealed class ApplicationContexts<T> :
		ApplicationContexts<AzureFunctionContext<T>, AzureFunctionParameter, IActionResult>,
		IApplicationContexts where T : class, ISelect<AzureFunctionParameter, IActionResult>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Start.A.Selection<AzureFunctionParameter>().By.Default<IServices>()) {}
	}
}