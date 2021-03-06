﻿using System;
using Super.Model.Results;
using Super.Runtime.Environment;

namespace Super.Compose.Results
{
	public sealed class Location<T> : Model.Selection.Adapters.Result<T>
	{
		public static Location<T> Default { get; } = new Location<T>();

		Location() : base(() => DefaultComponent<T>.Default) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public IResult<T> Throw() => DefaultComponentLocator<T>.Default;

			public IResult<T> Default(T instance) => new Component<T>(instance.Start());

			public IResult<T> Default(Func<T> result) => new Component<T>(result);
		}
	}
}