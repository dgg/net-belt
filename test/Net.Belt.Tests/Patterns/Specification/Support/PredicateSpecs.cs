using Net.Belt.Patterns.Specification;

namespace Net.Belt.Tests.Patterns.Specification.Support;

internal class LessThan10SpecSubject() : PredicateSpecification<int>(i => i < 10);

internal class MoreThan5SpecSubject() : PredicateSpecification<int>(i => i > 5);

internal class MoreThan10SpecSubject() : PredicateSpecification<int>(i => i > 10);

internal class LessThan5SpecSubject() : PredicateSpecification<int>(i => i < 5);

internal class BarEven() : PredicateSpecification<ComplexType>(item => item.Bar % 2 != 0);

internal class ComplexTypeEnabled() : PredicateSpecification<ComplexType>(c => c.Enabled);