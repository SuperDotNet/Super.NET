﻿using System;
using System.Threading.Tasks;
using Super.Model.Selection.Adapters;

namespace Super.Application.Services.Communication
{
	public static class ExtensionMethods
	{
		public static Selector<TIn, TOut> Request<TIn, T, TOut>(this Selector<TIn, T> @this,
		                                                        Func<T, Task<TOut>> parameter)
			=> @this.Select(new Request<T, TOut>(parameter))
			        .Select(Request<TOut>.Default);
	}
}