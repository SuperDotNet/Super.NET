using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Runtime.Environment
{
	sealed class ComponentType : DecoratedSelect<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.FirstAssigned()) {}
	}

	sealed class ComponentTypesDefinition : DecoratedSource<ISelect<Type, Array<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default, CanActivate.Default.IsSatisfiedBy) {}

		public ComponentTypesDefinition(IArray<Type> types, Func<Type, bool> where)
			: base(types.Out(x => x.Sequence())
			            .Where(where)
			            .Result()
			            .Select(I<ComponentTypesSelector>.Default)
			            .Select(x => x.Fixed().Sort().Result())
			            .Out()) {}
	}

	sealed class ComponentTypes : DelegatedInstanceSelector<Type, Array<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(ComponentTypesDefinition.Default.Select(x => x.ToStore()).ToContextual()) {}
	}

	sealed class SourceDefinition : MakeGenericType
	{
		public static SourceDefinition Default { get; } = new SourceDefinition();

		SourceDefinition() : base(typeof(ISource<>)) {}
	}

	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateMarker<Array<Type>>
	{
		readonly ImmutableArray<Type>            _types;
		readonly ISpecification<Type>            _specification;
		readonly Func<Type, ISelect<Type, Type>> _selections;

		[UsedImplicitly]
		public ComponentTypesSelector(Array<Type> types)
			: this(types, IsAssigned<Type>.Default, Selections.Default.Get) {}

		public ComponentTypesSelector(ImmutableArray<Type> types,
		                              ISpecification<Type> specification,
		                              Func<Type, ISelect<Type, Type>> selections)
		{
			_types         = types;
			_specification = specification;
			_selections    = selections;
		}

		public IEnumerable<Type> Get(Type parameter)
		{
			var select = _selections(parameter);
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				var type = select.Get(_types[i]);
				if (_specification.IsSatisfiedBy(type))
				{
					yield return type;
				}
			}
		}
	}
}