using System;

namespace Super.Model.Sources.Alterations
{
	public class DelegatedAlteration<T> : DelegatedSource<T, T>, IAlteration<T>
	{
		public DelegatedAlteration(Func<T, T> alteration) : base(alteration) {}
	}
}