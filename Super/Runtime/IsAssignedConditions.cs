﻿using System;
using System.Reflection;
using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Reflection.Types;

namespace Super.Runtime
{
	sealed class IsAssignedConditions<T> : ReferenceValueStore<TypeInfo, Func<T, bool>>
	{
		readonly static Type Type = AccountForUnassignedType.Default.Get(A.Type<T>());

		[UsedImplicitly]
		public static IsAssignedConditions<T> Default { get; } = new IsAssignedConditions<T>();

		IsAssignedConditions()
			: this(IsAssignableStructure.Default.Get(Type)
				       ? Start.A.Generic(typeof(HasValue<>))
				              .Of.Type<T>()
				              .As.Condition()
				              .Then()
				              .Invoke()
				              .Get()
				              .In(Type<T>.Instance.Yield().Result())
				              .Assume()
				       : IsReference.Default.Get(Type)
					       ? Start.A.Selection<T>()
					              .AndOf<object>()
					              .By.Cast.Select(IsAssigned.Default)
					       : IsModified<T>.Default) {}

		public IsAssignedConditions(ISelect<T, bool> condition)
			: base(Start.A.Selection.Of.System.Metadata.By.Returning(condition.ToCondition())
			            .Select(x => x.ToDelegate())) {}
	}
}