﻿using Super.Model.Results;

namespace Super.Runtime.Activation
{
	sealed class ReferenceActivator<T> : Result<object>, IActivator<object> where T : class
	{
		public static ReferenceActivator<T> Default { get; } = new ReferenceActivator<T>();

		ReferenceActivator() : base(Activator<T>.Default) {}
	}
}