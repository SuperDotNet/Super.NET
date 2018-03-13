using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class ConstructorLocator : ISource<TypeInfo, ConstructorInfo>
	{
		public static ConstructorLocator Default { get; } = new ConstructorLocator();

		ConstructorLocator() : this(ConstructorSpecification.Default) {}

		readonly ISource<TypeInfo, IEnumerable<ConstructorInfo>> _constructors;
		readonly ISpecification<ConstructorInfo>                 _specification;

		public ConstructorLocator(ISpecification<ConstructorInfo> specification)
			: this(specification, InstanceConstructors.Default) {}

		public ConstructorLocator(ISpecification<ConstructorInfo> specification,
		                          ISource<TypeInfo, IEnumerable<ConstructorInfo>> constructors)
		{
			_specification = specification;
			_constructors  = constructors;
		}

		public ConstructorInfo Get(TypeInfo parameter)
		{
			var constructors = _constructors.Get(parameter).ToArray();
			var length       = constructors.Length;
			for (var i = 0; i < length; i++)
			{
				var constructor = constructors[i];
				if (_specification.IsSatisfiedBy(constructor))
				{
					return constructor;
				}
			}

			return null;
		}
	}
}