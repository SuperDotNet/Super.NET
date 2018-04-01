﻿namespace Super.Model.Instances
{
	sealed class Default<T> : Instance<T>
	{
		public static Default<T> Instance { get; } = new Default<T>();

		Default() : base(default) {}
	}
}