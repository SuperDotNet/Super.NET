﻿using Super.Reflection.Types;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection
{
	sealed class Declared<T> : IDeclared<T>
	{
		public static Declared<T> Default { get; } = new Declared<T>();

		public static Declared<T> Inherited { get; } = new Declared<T>(true);

		Declared() : this(false) {}

		readonly bool _inherit;

		readonly Type _type;

		public Declared(bool inherit) : this(Type<T>.Instance, inherit) {}

		public Declared(Type type, bool inherit)
		{
			_type    = type;
			_inherit = inherit;
		}

		public ReadOnlyMemory<T> Get(ICustomAttributeProvider parameter)
			=> parameter.GetCustomAttributes(_type, _inherit).Cast<T>().ToArray();
	}
}