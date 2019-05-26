using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	public interface IFormattedProjection<in T> : ISelect<string, T, IProjection> {}
}