using System;
using System.Buffers;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	readonly struct Views<T>
	{
		readonly View<View<T>> _views;
		readonly ArrayPool<T>  _pool;

		/*public Indexing(View<T> first, ArrayPool<T> pool) : this(pool) {}*/

		public Views(View<View<T>> views, ArrayPool<T> pool)
		{
			_views = views;
			_pool  = pool;
		}

		public View<T> Compile(IEnumerator<T> parameter)
		{
			var  views = _views;
			var  total = views[0].Total;
			var  pages = 1u;
			bool next;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var view   = new View<T>(_pool.Rent((int)size), _pool);
				var target = view.Total;
				var local  = 0u;
				while (local < target && parameter.MoveNext())
				{
					view[local++] = parameter.Current;
					total++;
				}

				views[pages++] = view.Resize(in local);
				next           = local == target;
			} while (next);

			var store  = _pool.Rent((int)total);
			var master = new View<T>(store, _pool);
			var index = 0u;
			for (var i = 0u; i < pages; i++)
			{
				var view = views[i];
				master.Copy(in view, in index);
				index += view.Used;
				view.Release();
			}

			views.Release();
			return master.Resize(index);
		}
	}
}