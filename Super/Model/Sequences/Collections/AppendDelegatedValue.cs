﻿using System.Collections.Generic;
using Super.Model.Results;
using Super.Model.Selection.Alterations;

namespace Super.Model.Sequences.Collections
{
	public sealed class AppendDelegatedValue<T> : IAlteration<IEnumerable<T>>
	{
		readonly IResult<T> _item;

		public AppendDelegatedValue(IResult<T> item) => _item = item;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Append(_item.Get());
	}
}