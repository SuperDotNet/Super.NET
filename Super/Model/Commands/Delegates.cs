using System;
using Super.Model.Selection.Stores;

namespace Super.Model.Commands
{
	sealed class Delegates<T> : ReferenceValueStore<ICommand<T>, Action<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Execute) {}
	}
}