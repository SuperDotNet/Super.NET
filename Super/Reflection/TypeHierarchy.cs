using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Collections;
using Super.Model.Selection;

namespace Super.Reflection
{
	public sealed class TypeHierarchy : ISelect<TypeInfo, ImmutableArray<TypeInfo>>
	{
		public static TypeHierarchy Default { get; } = new TypeHierarchy();

		TypeHierarchy() {}

		public ImmutableArray<TypeInfo> Get(TypeInfo parameter) => new Hierarchy(parameter).ToImmutableArray();

		sealed class Hierarchy : ItemsBase<TypeInfo>
		{
			readonly TypeInfo _type;

			public Hierarchy(TypeInfo type) => _type = type;

			public override IEnumerator<TypeInfo> GetEnumerator()
			{
				yield return _type;
				var current = _type.BaseType;
				while (current != null)
				{
					var info = current.GetTypeInfo();
					if (current != typeof(object))
					{
						yield return info;
					}

					current = info.BaseType;
				}
			}
		}
	}
}