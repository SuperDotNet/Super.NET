using System.Collections.Generic;
using AutoFixture;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Runtime;

namespace Super.Testing.Objects
{
	public static class Extensions
	{
		public static ISelect<None, IEnumerable<T>> Many<T>(this IResult<IFixture> @this, uint count)
			=> @this.ToSelect().Select(new Many<T>(count));
	}
}