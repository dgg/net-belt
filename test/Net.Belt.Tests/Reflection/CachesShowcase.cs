using Dumpify;

using Net.Belt.Reflection;

namespace Net.Belt.Tests.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
internal class CustomMultiAttribute(string location) : CachedMultiAttribute<CustomMultiAttribute>
{
	public string Location { get; } = location;
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
internal class CustomAttribute : CachedAttribute<CustomAttribute>;

internal class NotDecorated;

[Custom, CustomMulti("base")]
internal class DecoratedBase;

[CustomMulti("derived_first"), CustomMulti("derived_second")]
internal class DecoratedDerived(string location) : DecoratedBase
{
	public string Location { get; } = location;
}

[TestFixture, Category("showcase"), Explicit("noisy")]
public class CachesShowcase
{
	[Test]
	public void Inheritance_CacheAccess()
	{
		CustomAttribute.Cache<DecoratedBase>.Value.Dump("cached attribute if decorated");
		
		CustomAttribute.ExactCache<DecoratedDerived>.Value.Dump("no attribute in type");
		CustomAttribute.Cache<DecoratedDerived>.Value.Dump("but reachable in base class");

		CustomMultiAttribute.ExactCache<DecoratedDerived>.Values.Dump("the ones from the type");
		CustomMultiAttribute.Cache<DecoratedDerived>.Values.Dump("even more from base classes");

		CustomMultiAttribute.Cache<DecoratedDerived>.First.Dump("from derived class");
	}
	
	[Test]
	public void InheritanceFree_CacheAccess()
	{
		Cache<CustomAttribute>.For<DecoratedBase>.Value.Dump("cached attribute if decorated");
		
		Cache<CustomAttribute>.ForExact<DecoratedDerived>.Value.Dump("no attribute in type");
		Cache<CustomAttribute>.For<DecoratedDerived>.Value.Dump("but reachable in base class");

		MultiCache<CustomMultiAttribute>.ForExact<DecoratedDerived>.Values.Dump("the ones from the type");
		MultiCache<CustomMultiAttribute>.For<DecoratedDerived>.Values.Dump("even more from base classes");

		MultiCache<CustomMultiAttribute>.For<DecoratedDerived>.First.Dump("from derived class");
	}
}