using System.Collections.Generic;
using AutoFixture;
using Super.Model.Selection;

namespace Super.Testing.Objects
{
	sealed class Many<T> : ISelect<IFixture, IEnumerable<T>>
	{
		readonly int _count;

		public Many(uint count) => _count = (int)count;

		public IEnumerable<T> Get(IFixture parameter) => parameter.CreateMany<T>(_count);
	}
}