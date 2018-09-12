﻿using Super.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	sealed class Repeat<T> : Enumerable<T>
	{
		readonly static Func<T> Create = New<T>.Default.ToDelegateReference();

		readonly int     _times;
		readonly Func<T> _create;

		public Repeat(int times) : this(times, Create) {}

		public Repeat(int times, Func<T> create)
		{
			_times  = times;
			_create = create;
		}

		public override IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < _times; i++)
			{
				yield return _create();
			}
		}
	}}