namespace Net.Belt.Tests.Reflection.Support;

[InheritedSingle("base")]
internal class InheritedSingleBase;
[InheritedSingle("derived")]
internal class InheritedSingleDerived : InheritedSingleBase;
internal class InheritedSingleNotDecorated : InheritedSingleBase;

[NotInheritedSingle("base")]
internal class NotInheritedSingleBase;
[NotInheritedSingle("derived")]
internal class NotInheritedSingleDerived : NotInheritedSingleBase;
internal class NotInheritedSingleNotDecorated : NotInheritedSingleBase;

[NotInheritedMulti("base")]
internal class NotInheritedMultiBase;
[NotInheritedMulti("derived")]
internal class NotInheritedMultiDerived : NotInheritedMultiBase;
[NotInheritedMulti("one"), NotInheritedMulti("two")]
internal class NotInheritedMulti;
internal class NotInheritedMultiNotDecorated : NotInheritedMultiBase;

[InheritedMulti("base")]
internal class InheritedMultiBase {}
[InheritedMulti("derived")]
internal class InheritedMultiDerived : InheritedMultiBase;
[InheritedMulti("one"), InheritedMulti("two")]
internal class InheritedMulti;
internal class InheritedMultiNotDecorated : InheritedMultiBase;

internal class NotDecorated;