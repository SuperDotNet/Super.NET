﻿using Super.ExtensionMethods;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	class Store<TParameter, TResult> : DelegatedSource<TParameter, TResult>
	{
		public Store() : this(Activation<TParameter, TResult>.Default.ToDelegate()) {}

		public Store(Func<TParameter, TResult> source) : base(Stores<TParameter, TResult>.Default.Get(source).Get) {}
	}
}