﻿using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Reflection.Types;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection
{
	sealed class ProvidedAttributes<T> : ISelect<ICustomAttributeProvider, Array<T>> where T : Attribute
	{
		public static ProvidedAttributes<T> Default { get; } = new ProvidedAttributes<T>();

		public static ProvidedAttributes<T> Inherited { get; } = new ProvidedAttributes<T>(true);

		ProvidedAttributes() : this(false) {}

		readonly bool _inherit;

		readonly Type _type;

		public ProvidedAttributes(bool inherit) : this(Type<T>.Instance, inherit) {}

		public ProvidedAttributes(Type type, bool inherit)
		{
			_type    = type;
			_inherit = inherit;
		}

		public Array<T> Get(ICustomAttributeProvider parameter)
			=> parameter.GetCustomAttributes(_type, _inherit)
			            .Cast<T>()
			            .ToArray();
	}
}