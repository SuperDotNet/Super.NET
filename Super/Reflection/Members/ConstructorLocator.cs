using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Members
{
	sealed class ConstructorLocator : ISelect<TypeInfo, ConstructorInfo>
	{
		public static ConstructorLocator Default { get; } = new ConstructorLocator();

		ConstructorLocator() : this(ConstructorCondition.Default) {}

		readonly ISelect<TypeInfo, IEnumerable<ConstructorInfo>> _constructors;
		readonly ICondition<ConstructorInfo>                     _condition;

		public ConstructorLocator(ICondition<ConstructorInfo> condition)
			: this(condition, InstanceConstructors.Default) {}

		public ConstructorLocator(ICondition<ConstructorInfo> condition,
		                          ISelect<TypeInfo, IEnumerable<ConstructorInfo>> constructors)
		{
			_condition    = condition;
			_constructors = constructors;
		}

		public ConstructorInfo Get(TypeInfo parameter)
		{
			var constructors = _constructors.Get(parameter).ToArray();
			var length       = constructors.Length;
			for (var i = 0u; i < length; i++)
			{
				var constructor = constructors[i];
				if (_condition.Get(constructor))
				{
					return constructor;
				}
			}

			return null;
		}
	}
}