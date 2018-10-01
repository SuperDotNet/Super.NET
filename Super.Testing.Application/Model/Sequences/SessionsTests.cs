using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class SessionsTests
	{
		[Fact]
		void Verify() {}

		/*sealed class StructureChains<T> where T : struct
		{
			public IStructure<T> Self { get; }

			public IStructure<T, T> Once { get; }

			public IStructure<T, T> Twice { get; }

			public static StructureChains<T> Default { get; } = new StructureChains<T>();

			StructureChains() : this(Super.Model.Selection.Structure.Self<T>.Default) {}

			public StructureChains(IStructure<T> self) :
				this(self, self.Select(Super.Model.Selection.Structure.Self<T>.Default)) {}

			public StructureChains(IStructure<T> self, IStructure<T, T> once)
				: this(self, once, once.Select(Super.Model.Selection
				                                    .Structure
				                                    .Self<T>
				                                    .Default)) {}

			public StructureChains(IStructure<T> self, IStructure<T, T> once, IStructure<T, T> twice)
			{
				Self  = self;
				Once  = once;
				Twice = twice;
			}
		}

		sealed class LocalChains<T> where T : struct
		{
			public static LocalChains<T> Default { get; } = new LocalChains<T>();

			LocalChains() : this(Super.Model.Selection.Self<ILocal<T>>.Default) {}

			public LocalChains(ISelect<ILocal<T>, ILocal<T>> self)
				: this(self, self.Select(Super.Model.Selection.Self<ILocal<T>>.Default)) {}

			public LocalChains(ISelect<ILocal<T>, ILocal<T>> self, ISelect<ILocal<T>, ILocal<T>> once)
				: this(self, once, once.Select(Super.Model.Selection.Self<ILocal<T>>.Default)) {}

			public LocalChains(ISelect<ILocal<T>, ILocal<T>> self, ISelect<ILocal<T>, ILocal<T>> once,
			                   ISelect<ILocal<T>, ILocal<T>> twice)
			{
				Self  = self;
				Once  = once;
				Twice = twice;
			}

			public ISelect<ILocal<T>, ILocal<T>> Self { get; }

			public ISelect<ILocal<T>, ILocal<T>> Once { get; }

			public ISelect<ILocal<T>, ILocal<T>> Twice { get; }
		}

		/*sealed class ReferenceChains<T> where T : struct
		{
			public static ReferenceChains<T> Default { get; } = new ReferenceChains<T>();

			ReferenceChains() : this(EphemeralSelf<T>.Default) {}

			public ReferenceChains(IEphemeral<T> self)
				: this(self, self.Select(EphemeralSelf<T>.Default)) {}

			public ReferenceChains(IEphemeral<T> self, IEphemeral<T, T> once)
				: this(self, once, once.Select(EphemeralSelf<T>.Default)) {}

			public ReferenceChains(IEphemeral<T> self, IEphemeral<T, T> once, IEphemeral<T, T> twice)
			{
				Self  = self;
				Once  = once;
				Twice = twice;
			}

			public IEphemeral<T> Self { get; }

			public IEphemeral<T, T> Once { get; }

			public IEphemeral<T, T> Twice { get; }
		}#1#

		sealed class Chains<T>
		{
			public static Chains<T> Default { get; } = new Chains<T>();

			Chains() : this(Super.Model.Selection.Self<T>.Default) {}

			public IAlteration<T> Self { get; }

			public ISelect<T, T> Once { get; }

			public ISelect<T, T> Twice { get; }

			public Chains(IAlteration<T> self) : this(self, self.Select(Super.Model.Selection.Self<T>.Default)) {}

			public Chains(IAlteration<T> self, ISelect<T, T> once)
				: this(self, once, once.Select(Super.Model.Selection.Self<T>.Default)) {}

			public Chains(IAlteration<T> self, ISelect<T, T> once, ISelect<T, T> twice)
			{
				Self  = self;
				Once  = once;
				Twice = twice;
			}
		}

		public class Benchmarks
		{
			readonly static Chains<Class> ReferenceChains = Chains<Class>.Default;

			readonly static LocalChains<OneReference>  LocalChain     = LocalChains<OneReference>.Default;
			readonly static LocalChains<TwoReferences> LocalChainsTwo = LocalChains<TwoReferences>.Default;
			readonly static LocalChains<TwoReferencesAndField> LocalChainsTwoField =
				LocalChains<TwoReferencesAndField>.Default;
			readonly static LocalChains<ThreeReferences> LocalChainsThree = LocalChains<ThreeReferences>.Default;

			readonly static StructureChains<OneReference>  Chain     = StructureChains<OneReference>.Default;
			readonly static StructureChains<TwoReferences> ChainsTwo = StructureChains<TwoReferences>.Default;
			readonly static StructureChains<TwoReferencesAndField> ChainsTwoField =
				StructureChains<TwoReferencesAndField>.Default;
			readonly static StructureChains<ThreeReferences> ChainsThree = StructureChains<ThreeReferences>.Default;

			/*readonly static ReferenceChains<OneReference>  ReferenceChain     = ReferenceChains<OneReference>.Default;
			readonly static ReferenceChains<TwoReferences> ReferenceChainsTwo = ReferenceChains<TwoReferences>.Default;
			readonly static ReferenceChains<TwoReferencesAndField> ReferenceChainsTwoField =
				ReferenceChains<TwoReferencesAndField>.Default;
			readonly static ReferenceChains<ThreeReferences> ReferenceChainsThree =
				ReferenceChains<ThreeReferences>.Default;#1#

			readonly OneReference                  _one;
			readonly TwoReferences                 _two;
			readonly TwoReferencesAndField         _twoPlus;
			readonly ThreeReferences               _three;
			readonly Class                         _reference;
			readonly ILocal<OneReference>          _localOne;
			readonly ILocal<TwoReferences>         _localTwo;
			readonly ILocal<TwoReferencesAndField> _localTwoPlus;
			readonly ILocal<ThreeReferences>       _localThree;

			public Benchmarks() : this(new OneReference(null),
			                           new TwoReferences(null, null),
			                           new TwoReferencesAndField(null, null, 0),
			                           new ThreeReferences(null, null, null), new Class()) {}

			// ReSharper disable once TooManyDependencies
			public Benchmarks(OneReference one, TwoReferences two, TwoReferencesAndField twoPlus, ThreeReferences three,
			                  Class reference) : this(one, two, twoPlus, three, reference,
			                                          Locals.For(one), Locals.For(two), Locals.For(twoPlus),
			                                          Locals.For(three)) {}

			public Benchmarks(OneReference one, TwoReferences two, TwoReferencesAndField twoPlus, ThreeReferences three,
			                  Class reference, ILocal<OneReference> localOne, ILocal<TwoReferences> localTwo,
			                  ILocal<TwoReferencesAndField> localTwoPlus, ILocal<ThreeReferences> localThree)
			{
				_one          = one;
				_two          = two;
				_twoPlus      = twoPlus;
				_three        = three;
				_reference    = reference;
				_localOne     = localOne;
				_localTwo     = localTwo;
				_localTwoPlus = localTwoPlus;
				_localThree   = localThree;
			}

			/*[Benchmark]
			public object LocalReferenceOneField() => LocalChain.Self.Get(_localOne);

			[Benchmark]
			public object LocalReferenceOneChainedField() => LocalChain.Once.Get(_localOne);#1#

			// [Benchmark]
			public object LocalReferenceOneChainedTwiceField() => LocalChain.Twice.Get(_localOne);

			/*[Benchmark]
			public object LocalReferenceTwoField() => LocalChainsTwo.Self.Get(_localTwo);

			[Benchmark]
			public object LocalReferenceTwoChainedField() => LocalChainsTwo.Once.Get(_localTwo);#1#

			//[Benchmark]
			public object LocalReferenceTwoChainedTwiceField() => LocalChainsTwo.Twice.Get(_localTwo);

			/*[Benchmark]
			public object LocalReferenceTwoAndFieldField() => LocalChainsTwoField.Self.Get(_localTwoPlus);

			[Benchmark]
			public object LocalReferenceTwoAndFieldChainedField() => LocalChainsTwoField.Once.Get(_localTwoPlus);#1#

			// [Benchmark]
			public object LocalReferenceTwoAndFieldChainedTwiceField()
				=> LocalChainsTwoField.Twice.Get(_localTwoPlus);

			/*[Benchmark]
			public object LocalReferenceThreeField() => LocalChainsThree.Self.Get(_localThree);

			[Benchmark]
			public object LocalReferenceThreeChainedField() => LocalChainsThree.Once.Get(_localThree);#1#

			// [Benchmark]
			public object LocalReferenceThreeChainedTwiceField() => LocalChainsThree.Twice.Get(_localThree);

			/*[Benchmark]
			public object LocalReferenceOne() => LocalChain.Self.Get(Locals.For(in _one));

			[Benchmark]
			public object LocalReferenceOneChained() => LocalChain.Once.Get(Locals.For(in _one));#1#

			[Benchmark]
			public object LocalReferenceOneChainedTwice() => LocalChain.Twice.Get(Locals.For(in _one));

			/*[Benchmark]
			public object LocalReferenceTwo() => LocalChainsTwo.Self.Get(Locals.For(in _two));

			[Benchmark]
			public object LocalReferenceTwoChained() => LocalChainsTwo.Once.Get(Locals.For(in _two));#1#

			[Benchmark]
			public object LocalReferenceTwoChainedTwice() => LocalChainsTwo.Twice.Get(Locals.For(in _two));

			/*[Benchmark]
			public object LocalReferenceTwoAndField() => LocalChainsTwoField.Self.Get(Locals.For(in _twoPlus));

			[Benchmark]
			public object LocalReferenceTwoAndFieldChained() => LocalChainsTwoField.Once.Get(Locals.For(in _twoPlus));#1#

			[Benchmark]
			public object LocalReferenceTwoAndFieldChainedTwice() => LocalChainsTwoField.Twice.Get(Locals.For(in _twoPlus));

			/*[Benchmark]
			public object LocalReferenceThree() => LocalChainsThree.Self.Get(Locals.For(in _three));

			[Benchmark]
			public object LocalReferenceThreeChained() => LocalChainsThree.Once.Get(Locals.For(in _three));#1#

			[Benchmark]
			public object LocalReferenceThreeChainedTwice() => LocalChainsThree.Twice.Get(Locals.For(in _three));

			/*[Benchmark]
			public void ReferenceValueOne() => ReferenceChain.Self.Get(in _one);

			[Benchmark]
			public void ReferenceValueOneChained() => ReferenceChain.Once.Get(in _one);

			[Benchmark]
			public void ReferenceValueOneChainedTwice() => ReferenceChain.Twice.Get(in _one);

			[Benchmark]
			public void ReferenceValueTwo() => ReferenceChainsTwo.Self.Get(in _two);

			[Benchmark]
			public void ReferenceValueTwoChained() => ReferenceChainsTwo.Once.Get(in _two);

			[Benchmark]
			public void ReferenceValueTwoChainedTwice() => ReferenceChainsTwo.Twice.Get(in _two);

			[Benchmark]
			public void ReferenceValueTwoAndField() => ReferenceChainsTwoField.Self.Get(in _twoPlus);

			[Benchmark]
			public void ReferenceValueTwoAndFieldChained() => ReferenceChainsTwoField.Once.Get(in _twoPlus);

			[Benchmark]
			public void ReferenceValueTwoAndFieldChainedTwice() => ReferenceChainsTwoField.Twice.Get(in _twoPlus);

			[Benchmark]
			public void ReferenceValueThree() => ReferenceChainsThree.Self.Get(in _three);

			[Benchmark]
			public void ReferenceValueThreeChained() => ReferenceChainsThree.Once.Get(in _three);

			[Benchmark]
			public void ReferenceValueThreeChainedTwice() => ReferenceChainsThree.Twice.Get(in _three);

			[Benchmark]
			public void One() => Chain.Self.Get(in _one);

			[Benchmark]
			public void OneChained() => Chain.Once.Get(in _one);

			[Benchmark]
			public void OneChainedTwice() => Chain.Twice.Get(in _one);

			[Benchmark]
			public void Two() => ChainsTwo.Self.Get(in _two);

			[Benchmark]
			public void TwoChained() => ChainsTwo.Once.Get(in _two);

			[Benchmark]
			public void TwoChainedTwice() => ChainsTwo.Twice.Get(in _two);

			[Benchmark]
			public void TwoAndField() => ChainsTwoField.Self.Get(in _twoPlus);

			[Benchmark]
			public void TwoAndFieldChained() => ChainsTwoField.Once.Get(in _twoPlus);

			[Benchmark]
			public void TwoAndFieldChainedTwice() => ChainsTwoField.Twice.Get(in _twoPlus);

			[Benchmark]
			public void Three() => ChainsThree.Self.Get(in _three);

			[Benchmark]
			public void ThreeChained() => ChainsThree.Once.Get(in _three);

			[Benchmark]
			public void ThreeChainedTwice() => ChainsThree.Twice.Get(in _three);#1#

			/*[Benchmark]
			public object Reference() => ReferenceChains.Self.Get(_reference);

			[Benchmark]
			public object ReferenceChained() => ReferenceChains.Once.Get(_reference);#1#

			[Benchmark]
			public object ReferenceChainedTwice() => ReferenceChains.Twice.Get(_reference);
		}

		public sealed class Class
		{
			public Class() : this(new object(), new object(), new object(), new object(), new object(), new object()) {}

			// ReSharper disable once TooManyDependencies
			public Class(object one, object two, object three, object four, object five, object six)
			{
				One   = one;
				Two   = two;
				Three = three;
				Four  = four;
				Five  = five;
				Six   = six;
			}

			public object One { get; }

			public object Two { get; }

			public object Three { get; }

			public object Four { get; }

			public object Five { get; }

			public object Six { get; }
		}

		public readonly struct OneReference
		{
			public OneReference(object reference) => Reference = reference;

			public object Reference { get; }
		}

		public readonly struct TwoReferences
		{
			public TwoReferences(object one, object two)
			{
				One = one;
				Two = two;
			}

			public object One { get; }

			public object Two { get; }
		}

		public readonly struct TwoReferencesAndField
		{
			public TwoReferencesAndField(object one, object two, uint field)
			{
				One   = one;
				Two   = two;
				Field = field;
			}

			public object One { get; }

			public object Two { get; }

			public uint Field { get; }
		}

		public readonly struct ThreeReferences
		{
			public object One { get; }
			public object Two { get; }
			public object Three { get; }

			public ThreeReferences(object one, object two, object three)
			{
				One   = one;
				Two   = two;
				Three = three;
			}
		}*/
	}
}