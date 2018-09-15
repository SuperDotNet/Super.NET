using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	readonly struct Views<T>
	{
		readonly View<View<T>> _views;

		public Views(View<View<T>> views) => _views = views;

		public View<T> Compile(IEnumerator<T> parameter)
		{
			var  views = _views;
			var array = views.Source;
			var  first = array[0];
			var  total = first.Available;
			var  pages = 1u;
			bool next;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var view   = first.New(size);
				var target = view.Available;
				var local  = 0u;
				var source = view.Source;
				while (local < target && parameter.MoveNext())
				{
					source[local++] = parameter.Current;
					total++;
				}

				array[pages++] = view.Resize(in local);
				next           = local == target;
			} while (next);

			var master = first.New(total);
			var offset = 0u;
			for (var i = 0u; i < pages; i++)
			{
				var view = array[i];
				master.Copy(in view, in offset);
				offset += view.Used;
				view.Release();
			}

			views.Release();
			return master.Resize(offset);
		}
	}
}