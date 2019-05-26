using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using System;

namespace Super.Reflection.Types
{
	sealed class GenericInterfaces : Store<Type, Array<Type>>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : base(AllInterfaces.Default.Query()
		                                        .Where(y => y.IsGenericType)
		                                        .Get) {}
	}
}