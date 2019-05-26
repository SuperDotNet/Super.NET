using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	sealed class Result<T> : Select<IEnumerable<T>, Array<T>>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() : base(x => x.Open()) {}
	}
}