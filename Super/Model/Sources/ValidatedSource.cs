﻿using System;
using Super.ExtensionMethods;
using Super.Model.Specifications;

namespace Super.Model.Sources
{
	class ValidatedSource<T> : ISource<T>
	{
		readonly Func<T, bool> _specification;
		readonly Func<T> _instance, _fallback;

		public ValidatedSource(ISpecification<T> specification, ISource<T> instance, ISource<T> fallback)
			: this(specification.ToDelegate(), instance.ToDelegate(), fallback.ToDelegate()) {}

		public ValidatedSource(Func<T, bool> specification, Func<T> instance, Func<T> fallback)
		{
			_specification = specification;
			_instance = instance;
			_fallback = fallback;
		}

		public T Get()
		{
			var instance = _instance();
			var result = _specification(instance) ? instance : _fallback();
			return result;
		}
	}
}