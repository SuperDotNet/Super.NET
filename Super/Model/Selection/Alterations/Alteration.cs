using System;

namespace Super.Model.Selection.Alterations
{
	public class Alteration<T> : Select<T, T>, IAlteration<T>
	{
		public Alteration(ISelect<T, T> select) : this(select.Get) {}

		public Alteration(Func<T, T> alteration) : base(alteration) {}
	}
}