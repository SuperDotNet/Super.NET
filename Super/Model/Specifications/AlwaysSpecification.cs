﻿namespace Super.Model.Specifications
{
	public sealed class AlwaysSpecification<T> : ISpecification<T>
	{
		public static AlwaysSpecification<T> Default { get; } = new AlwaysSpecification<T>();

		AlwaysSpecification() {}

		public bool IsSatisfiedBy(T parameter) => true;
	}
}