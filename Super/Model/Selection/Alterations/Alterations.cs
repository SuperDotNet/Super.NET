using System;
using Super.Model.Selection.Stores;

namespace Super.Model.Selection.Alterations
{
	sealed class Alterations<T> : ReferenceValueStore<Func<T, T>, IAlteration<T>>
	{
		public static Alterations<T> Default { get; } = new Alterations<T>();

		Alterations() : base(x => x.Target as IAlteration<T> ?? new Alteration<T>(x)) {}
	}
}