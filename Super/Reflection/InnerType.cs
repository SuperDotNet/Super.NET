using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources.Alterations;
using Super.Model.Specifications;

namespace Super.Reflection
{
	public sealed class InnerType : IAlteration<TypeInfo>
	{
		public static InnerType Default { get; } = new InnerType();

		InnerType() : this(AlwaysSpecification<TypeInfo>.Default) {}

		readonly Func<TypeInfo, ImmutableArray<TypeInfo>> _hierarchy;
		readonly Func<Type[], TypeInfo>                   _select;
		readonly ISpecification<TypeInfo>                 _specification;

		public InnerType(ISpecification<TypeInfo> specification) : this(specification, TypeHierarchy.Default.Get,
		                                                                x => x.Only()?.GetTypeInfo()) {}

		public InnerType(ISpecification<TypeInfo> specification, Func<TypeInfo, ImmutableArray<TypeInfo>> hierarchy,
		                 Func<Type[], TypeInfo> select)
		{
			_specification = specification;
			_hierarchy     = hierarchy;
			_select        = select;
		}

		public TypeInfo Get(TypeInfo parameter)
		{
			var hierarchy = _hierarchy(parameter);
			foreach (var info in hierarchy)
			{
				var result = info.IsGenericType && info.GenericTypeArguments.Any() && _specification.IsSatisfiedBy(info)
					             ? _select(info.GenericTypeArguments)
					             : info.IsArray
						             ? info.GetElementType()
						                   .GetTypeInfo()
						             : null;
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}

		//public Type GetEnumerableType() => InnerType(GetHierarchy(), types => types.Only(), i => i.Adapt().IsGenericOf(typeof(IEnumerable<>)));
	}
}