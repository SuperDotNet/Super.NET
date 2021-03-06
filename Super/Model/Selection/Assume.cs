﻿using System;
using Super.Runtime.Activation;

namespace Super.Model.Selection
{
	public class Assume<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<Func<Func<TIn, TOut>>>
	{
		readonly Func<Func<TIn, TOut>> _source;

		public Assume(Func<Func<TIn, TOut>> source) => _source = source;

		public TOut Get(TIn parameter) => _source()(parameter);
	}
}