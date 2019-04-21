﻿using System;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Model.Selection.Conditions
{
	public class DelegatedResultCondition : DelegatedResultCondition<None>, ICondition
	{
		public DelegatedResultCondition(Func<bool> @delegate) : base(@delegate) {}
	}

	public class DelegatedResultCondition<T> : ICondition<T>, IActivateUsing<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public DelegatedResultCondition(Func<bool> @delegate) => _delegate = @delegate;

		public bool Get(T _) => _delegate();
	}
}