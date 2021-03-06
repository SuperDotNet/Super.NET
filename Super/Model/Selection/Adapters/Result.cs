﻿using System;
using Super.Runtime;

namespace Super.Model.Selection.Adapters
{
	public class Result<T> : Results.Result<T>, ISelect<T>
	{
		readonly Func<T> _source;

		public Result(Func<T> source) : base(source) => _source = source;

		public T Get(None _) => _source();

		public static implicit operator Result<T>(Func<T> value) => new Result<T>(value);

		public static implicit operator Func<T>(Result<T> value) => value.Get;
	}
}