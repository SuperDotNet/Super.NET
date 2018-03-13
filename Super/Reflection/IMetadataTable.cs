using System.Reflection;
using Super.Model.Sources.Tables;

namespace Super.Reflection
{
	public interface IMetadataTable<TMetadata, TValue> : ITable<TMetadata, TValue> where TMetadata : MemberInfo {}
}