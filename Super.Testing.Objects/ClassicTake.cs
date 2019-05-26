using System;
using System.Collections.Generic;
using System.Linq;
using Super.Model.Selection;
using Super.Runtime.Activation;

namespace Super.Testing.Objects
{
	sealed class ClassicTake<T> : ISelect<uint, IEnumerable<T>>, IActivateUsing<Func<IEnumerable<T>>>
	{
		readonly Func<IEnumerable<T>> _source;

		public ClassicTake(Func<IEnumerable<T>> source) => _source = source;

		public IEnumerable<T> Get(uint parameter) => _source().Take((int)parameter);
	}
}