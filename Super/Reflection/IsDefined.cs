using Super.Model.Specifications;
using Super.Reflection.Types;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class IsDefined<T> : ISpecification<ICustomAttributeProvider>
	{
		public static IsDefined<T> Default { get; } = new IsDefined<T>();

		public static IsDefined<T> Inherited { get; } = new IsDefined<T>(false);

		IsDefined() : this(true) {}

		readonly bool _inherit;

		readonly Type _type;

		public IsDefined(bool inherit) : this(Type<T>.Instance, inherit) {}

		public IsDefined(Type type, bool inherit)
		{
			_type    = type;
			_inherit = inherit;
		}

		public bool IsSatisfiedBy(ICustomAttributeProvider parameter) => parameter.IsDefined(_type, _inherit);
	}
}