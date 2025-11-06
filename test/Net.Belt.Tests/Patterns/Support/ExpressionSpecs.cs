using Net.Belt.Patterns;

namespace Net.Belt.Tests.Patterns.Support;

internal class LessThan10ExpSpecSubject() : ExpressionSpecification<int>(i => i < 10);

internal class MoreThan5ExpSpecSubject() : ExpressionSpecification<int>(i => i > 5);

internal class MoreThan10SpecExprSubject() : ExpressionSpecification<int>(s => s > 10);

internal class LessThan5SpecExprSubject() : ExpressionSpecification<int>(s => s < 5);

internal class EvenBar() : ExpressionSpecification<ComplexType>(item => item.Bar % 2 != 0);

internal class EnabledComplexType() : ExpressionSpecification<ComplexType>(c => c.Enabled);