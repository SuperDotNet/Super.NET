﻿using Super.Model.Sequences;

namespace Super.Runtime.Activation
{
	sealed class SingletonCandidates : ArrayInstance<string>, ISingletonCandidates
	{
		public static SingletonCandidates Default { get; } = new SingletonCandidates();

		SingletonCandidates() : this("Default", "Instance", "Implementation", "Singleton") {}

		public SingletonCandidates(params string[] items) : base(items) {}
	}
}