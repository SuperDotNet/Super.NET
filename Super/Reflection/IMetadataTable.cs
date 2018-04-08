using System.Reflection;
using Super.Model.Selection.Stores;

namespace Super.Reflection
{
	public interface IMetadataTable<TMetadata, TValue> : ITable<TMetadata, TValue> where TMetadata : MemberInfo {}
}