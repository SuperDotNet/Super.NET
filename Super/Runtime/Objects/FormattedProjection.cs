using System;
using Super.Model.Selection;
using Super.Text;

namespace Super.Runtime.Objects
{
	class FormattedProjection<T> : Selection<T, IProjection>, IFormattedProjection<T>
	{
		public FormattedProjection(ISelect<T, IProjection> @default, params Pair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}
}