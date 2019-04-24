using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Runtime.Environment
{
	sealed class ComponentType : Select<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.Query().FirstAssigned()) {}
	}

	sealed class ComponentTypesDefinition : Model.Results.Result<ISelect<Type, Array<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default.Query()
		                                       .Where(CanActivate.Default.Get)
		                                       .To(x => x.Get().Then())
		                                       .Activate<ComponentTypesSelector>()
		                                       .Select(x => x.Open().Then().Sort().Out())
		                                       .Selector()) {}

		public ComponentTypesDefinition(Func<ISelect<Type, Array<Type>>> source) : base(source) {}
	}

	sealed class ComponentTypes : Assume<Type, Array<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(A.This(ComponentTypesDefinition.Default)
		                         .Select(x => x.ToStore())
		                         .ToContextual()
		                         .AsDefined()
		                         .Then()
		                         .Delegate()
		                         .Selector()) {}
	}

	sealed class ResultDefinition : MakeGenericType
	{
		public static ResultDefinition Default { get; } = new ResultDefinition();

		ResultDefinition() : base(typeof(IResult<>)) {}
	}

	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateUsing<Array<Type>>
	{
		readonly ImmutableArray<Type>            _types;
		readonly ICondition<Type>                _condition;
		readonly Func<Type, ISelect<Type, Type>> _selections;

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