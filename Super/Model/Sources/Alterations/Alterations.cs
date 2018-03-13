using System;

namespace Super.Model.Sources.Alterations
{
	sealed class Alterations<T> : ReferenceStore<Func<T, T>, IAlteration<T>>
	{
		public static Alterations<T> Default { get; } = new Alterations<T>();

		Alterations() : base(x => x.Target as IAlteration<T> ?? new DelegatedAlteration<T>(x)) {}
	}
}