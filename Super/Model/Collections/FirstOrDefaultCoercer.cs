using Super.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class FirstOrDefaultCoercer<T> : DelegatedSource<IEnumerable<T>, T>
	{
		public static FirstOrDefaultCoercer<T> Default { get; } = new FirstOrDefaultCoercer<T>();

		FirstOrDefaultCoercer() : base(enumerable => enumerable.FirstOrDefault()) {}
	}
}