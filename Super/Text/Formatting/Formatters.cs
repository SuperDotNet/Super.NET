using System;
using Super.Runtime.Invocation;

namespace Super.Text.Formatting
{
	sealed class Formatters<T> : Invocation0<T, ISelectFormatter<T>, IFormattable>
	{
		public Formatters(ISelectFormatter<T> parameter)
			: base((instance, formatter) => new Adapter<T>(instance, formatter.Get), parameter) {}
	}
}