using Super.Model.Selection.Stores;
using System;

namespace Super.Model.AttachedProperties
{
	class Property<THost, TValue> : ReferenceStore<THost, TValue> where THost : class
	{
		public Property() : this(_ => default) {}

		public Property(Func<THost, TValue> source) : base(source) {}
	}
}