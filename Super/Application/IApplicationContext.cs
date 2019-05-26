using System;
using Super.Model.Commands;
using Super.Model.Selection;

namespace Super.Application
{
	public interface IApplicationContext : IDisposable {}

	public interface IApplicationContext<in T> : ICommand<T>, IApplicationContext {}

	public interface IApplicationContext<in TIn, out TOut> : ISelect<TIn, TOut>, IApplicationContext {}
}