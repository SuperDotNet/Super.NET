using System;
using Super.Model.Results;

namespace Super.Model.Sequences
{
	public class ArrayResult<T> : Results.Result<Array<T>>, IArray<T>
	{
		public ArrayResult(IResult<Array<T>> source) : this(source.Get) {}

		public ArrayResult(Func<Array<T>> source) : base(source) {}
	}
}