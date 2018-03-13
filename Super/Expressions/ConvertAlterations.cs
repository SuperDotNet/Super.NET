using System;
using Super.Model.Sources;

namespace Super.Expressions
{
	sealed class ConvertAlterations : ReferenceStore<Type, ConvertAlteration>
	{
		public static ConvertAlterations Default { get; } = new ConvertAlterations();

		ConvertAlterations() : base(x => new ConvertAlteration(x)) {}
	}
}