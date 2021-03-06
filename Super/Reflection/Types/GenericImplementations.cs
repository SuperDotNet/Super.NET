﻿using System;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	public class GenericImplementations : ISelect<Type, Array<Type>>
	{
		readonly Type                                           _definition;
		readonly ISelect<Type, IConditional<Type, Array<Type>>> _implementations;

		public GenericImplementations(Type definition) : this(GenericInterfaceImplementations.Default,
		                                                      definition) {}

		public GenericImplementations(ISelect<Type, IConditional<Type, Array<Type>>> implementations,
		                              Type definition)
		{
			_implementations = implementations;
			_definition      = definition;
		}

		public Array<Type> Get(Type parameter) => _implementations.Get(parameter)
		                                                          .Get(_definition)
		                                                          .Open();
	}
}