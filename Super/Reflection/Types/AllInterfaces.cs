using System;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	sealed class AllInterfaces : Store<Type, Array<Type>>
	{
		public static AllInterfaces Default { get; } = new AllInterfaces();

		AllInterfaces() : base(TypeMetadata.Default.Select(Interfaces.Default)
		                                   .Query()
		                                   .Where(y => y.IsInterface)
		                                   .Distinct()
		                                   .Get) {}
	}
}