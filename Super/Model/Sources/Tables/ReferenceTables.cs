﻿using System;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection;

namespace Super.Model.Sources.Tables
{
	public sealed class ReferenceTables<TParameter, TResult>
		: DelegatedSource<Func<TParameter, TResult>, ITable<TParameter, TResult>>
		where TParameter : class
	{
		public static ReferenceTables<TParameter, TResult> Default { get; } = new ReferenceTables<TParameter, TResult>();

		ReferenceTables() : this(IsValueTypeSpecification.Default.IsSatisfiedBy(typeof(TResult))
			                         ? typeof(StructureValueTable<,>)
			                         : typeof(ReferenceValueTable<,>)) {}

		public ReferenceTables(Type type)
			: base(new Generic<Func<TParameter, TResult>, ITable<TParameter, TResult>>(type)
			       .Get(typeof(TParameter), typeof(TResult))
			       .Invoke) {}
	}
}