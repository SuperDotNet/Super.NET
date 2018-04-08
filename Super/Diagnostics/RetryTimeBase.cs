﻿using System;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public abstract class RetryTimeBase : ISelect<int, TimeSpan>
	{
		readonly Alter<int> _time;

		protected RetryTimeBase(Alter<int> time) => _time = time;

		public TimeSpan Get(int parameter) => TimeSpan.FromSeconds(_time(parameter));
	}
}