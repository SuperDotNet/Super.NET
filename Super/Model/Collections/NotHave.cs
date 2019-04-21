using Super.Runtime.Activation;
using System.Collections.Generic;
using Super.Model.Selection.Conditions;

namespace Super.Model.Collections
{
	public class NotHave<T> : InverseCondition<T>, IActivateUsing<ICollection<T>>, IActivateUsing<IEnumerable<T>>
	{
		public NotHave(ICollection<T> source) : base(new Has<T>(source)) {}

		public NotHave(IEnumerable<T> source) : base(new Has<T>(source)) {}
	}
}