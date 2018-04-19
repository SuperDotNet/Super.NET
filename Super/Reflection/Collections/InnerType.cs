using Super.Model.Selection.Alterations;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace Super.Reflection.Collections
{
	public sealed class InnerType : IAlteration<TypeInfo>
	{
		public static InnerType Default { get; } = new InnerType();

		InnerType() : this(Always<TypeInfo>.Default) {}

		readonly Func<TypeInfo, ImmutableArray<TypeInfo>> _hierarchy;
		readonly Func<Type[], TypeInfo>                   _select;
		readonly ISpecification<TypeInfo>                 _specification;

		public InnerType(ISpecification<TypeInfo> specification) : this(HasGenericArguments.Default.And(specification),
		                                                                TypeHierarchy.Default.Get,
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
				var result = _specification.IsSatisfiedBy(info)
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
	}
}