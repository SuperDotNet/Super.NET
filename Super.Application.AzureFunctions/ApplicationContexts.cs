﻿using Microsoft.AspNetCore.Mvc;
using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Application.AzureFunctions
{
	sealed class ApplicationContexts<T> :
		ApplicationContexts<AzureFunctionContext<T>, AzureFunctionParameter, IActionResult>,
		IApplicationContexts where T : class, ISelect<AzureFunctionParameter, IActionResult>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Select.New<AzureFunctionParameter, AzureFunctionArgument>()
		                                   .Out(Services<AzureFunctionArgument>.Default)) {}
	}
}