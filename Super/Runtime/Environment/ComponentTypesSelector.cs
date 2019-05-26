using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateUsing<Array<Type>>
	{
		readonly ICondition<Type>                _condition;
		readonly Func<Type, ISelect<Type, Type>> _selections;
		readonly ImmutableArray<Type>            _types;

		[UsedImplicitly]
		public ComponentTypesSelector(Array<Type> types)
			: this(types, IsAssigned<Type>.Default, Selections.Default.Get) {}

		public ComponentTypesSelector(ImmutableArray<Type> types,
		                              ICondition<Type> condition,
		                              Func<Type, ISelect<Type, Type>> selections)
		{
			_types      = types;
			_condition  = condition;
			_selections = selections;
		}

		public IEnumerable<Type> Get(Type parameter)
		{
			var select = _selections(parameter);
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				var type = select.Get(_types[i]);
				if (_condition.Get(type))
				{
					yield return type;
				}
			}
		}
	}
}