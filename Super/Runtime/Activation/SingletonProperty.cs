﻿using System;
using System.Reflection;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;

namespace Super.Runtime.Activation
{
	sealed class SingletonProperty : ReferenceValueStore<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		public SingletonProperty(IResult<Array<string>> candidates)
			: base(Start.A.Selection.Of.System.Type.By.Delegate<string, PropertyInfo>(x => x.GetProperty)
			            .Select(candidates.Select)
			            .Then()
			            .Value()
			            .Query()
			            .Where(IsSingletonProperty.Default)
			            .FirstAssigned()
			            .Get) {}
	}
}