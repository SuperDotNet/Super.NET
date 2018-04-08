using System;

namespace Super.Model.Selection.Alterations
{
	public class DelegatedAlteration<T> : Delegated<T, T>, IAlteration<T>
	{
		public DelegatedAlteration(Func<T, T> alteration) : base(alteration) {}
	}
}