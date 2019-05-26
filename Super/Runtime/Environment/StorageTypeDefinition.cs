using System;
using Super.Model.Results;

namespace Super.Runtime.Environment
{
	public sealed class StorageTypeDefinition : Variable<Type>
	{
		public static StorageTypeDefinition Default { get; } = new StorageTypeDefinition();

		StorageTypeDefinition() : base(typeof(Variable<>)) {}
	}
}