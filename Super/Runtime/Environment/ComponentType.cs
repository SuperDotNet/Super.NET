using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Model.Selection;
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

	sealed class ComponentTypesDefinition : DecoratedSource<ISelect<Type, ReadOnlyMemory<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default, ComponentTypesPredicate.Default, x => x.Sort().Access()) {}

		public ComponentTypesDefinition(IArrays<Type> types, ISelectSequence<Type> where,
		                                Func<ISelect<Type, IEnumerable<Type>>, ISelect<Type, ReadOnlyMemory<Type>>>
			                                select)
			: base(types.Select(x => x.AsEnumerable())
			            .Select(where)
			            .Select(x => x.ToImmutableArray())
			            .Select(I<ComponentTypesSelector>.Default)
			            .Select(select)) {}
	}

	sealed class ComponentTypes : DelegatedInstanceSelector<Type, ReadOnlyMemory<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(ComponentTypesDefinition.Default.Select(x => x.ToStore()).ToContextual()) {}
	}

	sealed class ComponentTypesPredicate : WhereSelector<Type>
	{
		public static ComponentTypesPredicate Default { get; } = new ComponentTypesPredicate();

		ComponentTypesPredicate() : this(CanActivate.Default.IsSatisfiedBy) {}

		public ComponentTypesPredicate(Func<Type, bool> @where) : base(@where) {}
	}

	sealed class SourceDefinition : MakeGenericType
	{
		public static SourceDefinition Default { get; } = new SourceDefinition();

		SourceDefinition() : base(typeof(ISource<>)) {}
	}

	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateMarker<ImmutableArray<Type>>
	{
		readonly ImmutableArray<Type>            _types;
		readonly ISpecification<Type>            _specification;
		readonly Func<Type, ISelect<Type, Type>> _selections;

		[UsedImplicitly]
		public ComponentTypesSelector(ImmutableArray<Type> types)
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