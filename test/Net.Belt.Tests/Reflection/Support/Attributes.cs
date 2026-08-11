using Net.Belt.Reflection;

namespace Net.Belt.Tests.Reflection.Support;

[AttributeUsage(AttributeTargets.Class)]
internal class InheritedSingleAttribute(string subject) : CachedAttribute<InheritedSingleAttribute>
{
	public string Subject => subject;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal class NotInheritedSingleAttribute(string subject) : CachedAttribute<NotInheritedSingleAttribute>
{
	public string Subject => subject;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class NotInheritedMultiAttribute(string subject) : CachedMultiAttribute<NotInheritedMultiAttribute>
{
	public string Subject => subject;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class InheritedMultiAttribute(string subject) : CachedMultiAttribute<InheritedMultiAttribute>
{
	public string Subject => subject;
}

