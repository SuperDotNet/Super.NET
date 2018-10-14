﻿using Super.Model.Specifications;
using Super.Runtime.Activation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public class Has<T> : DelegatedSpecification<T>, IActivateMarker<ICollection<T>>, IActivateMarker<IEnumerable<T>>
	{
		public Has(ICollection<T> source) : base(source.Contains) {}

		public Has(IEnumerable<T> source) : base(source.Contains) {}
	}

	public class NotHave<T> : InverseSpecification<T>, IActivateMarker<ICollection<T>>, IActivateMarker<IEnumerable<T>>
	{
		public NotHave(ICollection<T> source) : base(new Has<T>(source)) {}

		public NotHave(IEnumerable<T> source) : base(new Has<T>(source)) {}
	}

	public sealed class HasAny : DelegatedSpecification<ICollection>
	{
		public static HasAny Default { get; } = new HasAny();

		HasAny() : base(x => x.Count > 0) {}
	}

	public sealed class HasAny<T> : DelegatedSpecification<ICollection<T>>
	{
		public static HasAny<T> Default { get; } = new HasAny<T>();

		HasAny() : base(x => x.Count > 0) {}
	}

	public sealed class HasNone<T> : InverseSpecification<ICollection<T>>
	{
		public static HasNone<T> Default { get; } = new HasNone<T>();

		HasNone() : base(HasAny<T>.Default) {}
	}
}