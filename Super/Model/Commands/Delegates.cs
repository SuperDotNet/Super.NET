using Super.Model.Sources;
using System;

namespace Super.Model.Commands
{
	sealed class Delegates<T> : ReferenceStore<ICommand<T>, Action<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Execute) {}
	}
}
