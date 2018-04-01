using System.Collections.Generic;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection.Query
{
	public sealed class PublicMembers : DelegatedSource<TypeInfo, IEnumerable<MemberInfo>>
	{
		public static PublicMembers Default { get; } = new PublicMembers();

		PublicMembers() : base(x => x.DeclaredMembers) {}
	}
}