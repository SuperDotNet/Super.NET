﻿using Super.Model.Selection.Alterations;
using Super.Reflection.Types;
using Super.Runtime.Objects;

namespace Super.Model.Selection
{
	sealed class Defaults<TParameter, TResult> : Conditional<ISelect<TParameter, TResult>, ISelect<TParameter, TResult>>
	{
		public static Defaults<TParameter, TResult> Default { get; } = new Defaults<TParameter, TResult>();

		Defaults() : base(IsType<ISelect<TParameter, TResult>, IAlteration<TParameter>>.Default,
		                  Cast<TParameter, TResult>.Default,
		                  Default<TParameter, TResult>.Instance) {}
	}
}