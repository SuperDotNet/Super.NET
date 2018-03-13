using System.Reflection;

namespace Super.Model.Collections
{
	sealed class DictionaryPairTypes
	{
		public DictionaryPairTypes(TypeInfo keyType, TypeInfo valueType)
		{
			KeyType   = keyType;
			ValueType = valueType;
		}

		public TypeInfo KeyType { get; }
		public TypeInfo ValueType { get; }
	}
}