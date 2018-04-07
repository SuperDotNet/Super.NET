using System;
using Super.Model.Specifications;

namespace Super.Reflection
{
	public static class Where<T>
	{
		public static Func<T, bool> Assigned => IsAssigned<T>.Default.IsSatisfiedBy;

		public static Func<T, bool> Always => AlwaysSpecification<T>.Default.IsSatisfiedBy;
	}
}