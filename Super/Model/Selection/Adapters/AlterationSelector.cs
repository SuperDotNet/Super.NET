using System;
using Super.Model.Selection.Alterations;

namespace Super.Model.Selection.Adapters
{
	public class AlterationSelector<T> : Selector<T, T>
	{
		public AlterationSelector(IAlteration<T> subject) : base(subject) {}

		public static implicit operator Func<T, T>(AlterationSelector<T> instance) => instance.Get().ToDelegate();
	}
}