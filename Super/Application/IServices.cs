using Super.Model.Selection;
using System;

namespace Super.Application
{
	public interface IServices : IServiceProvider {}

	public interface IServices<in T> : ISelect<T, IServices> {}
}