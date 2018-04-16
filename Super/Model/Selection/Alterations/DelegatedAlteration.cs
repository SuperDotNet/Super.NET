using System;

namespace Super.Model.Selection.Alterations
{
	public class DelegatedAlteration<T> : Select<T, T>, IAlteration<T>
	{
		public DelegatedAlteration(Func<T, T> alteration) : base(alteration) {}
	}
}