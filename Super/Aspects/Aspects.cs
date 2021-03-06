﻿using Super.Compose;
using Super.Model.Selection;
using Super.Runtime.Environment;

namespace Super.Aspects
{
	public sealed class Aspects<TIn, TOut> : Select<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		public static Aspects<TIn, TOut> Default { get; } = new Aspects<TIn, TOut>();

		Aspects() : base(Start.A.Selection<ISelect<TIn, TOut>>()
		                      .By.Type.Then()
		                      .Select(SystemStores.New(RegisteredAspects<TIn, TOut>.Default.Stores().New)
		                                          .Assume())) {}
	}
}