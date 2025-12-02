# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Project Overview

This project (_Net.Belt_ - or _Net-Belt_-) contains the implentation of helpers, patterns and extensions that help the development of general purpose applications (mostly web applications) using .NET (specifically .NET)

## Build and Test commands

* **Build:** `dotnet build` (from repository root)
* **Test All:** `dotnet test` (from repository root)
* **Run Single Test:** `dotnet test --filter "FullyQualifiedName~YourNamespace.YourClass.YourMethod"`
* when, refactoring and writing code avoid warnings
* **Linting/Code Style:** Enforced during build. Code style violations are treated as errors, causing the build to fail.

## General Instructions
* use English for all code, comments and documentation
* make only high confidence changes, avoid speculative changes
* write code with maintainability in mind, suing comments as support on why something is done a certain way
* when stuck:
  * ask a clarifying question or propose a short plan
  * do not make large speculative changes

## Code style guidelines
* follow the styling guides defined in the .editorconfig file at the repository root
* follow the C# coding guidelines defined in the .editorconfig file at the repository root
	* **Important Naming Conventions:**
		* private fields: `_camelCase` (e.g., `_myField`)
		* private static fields: `_camelCase` (e.g., `_myStaticField`)
		* local functions: `_camelCase` (e.g., `_myLocalFunction`)
* prefer using the latest C# features of the language version defined in the Net.Belt.csproj file
* prefer expression-bodied members where applicable
* prefer `var` keyword for local variable declarations when the type is obvious from the right side of the assignment, but use explicit types when the type is not obvious
* use target-typed `new` expressions where applicable
* don't use `this` keyword for instance members unless absolutely necessary to avoid ambiguity
* use file-scoped namespaces
* prefer braces for code blocks (`csharp_prefer_braces = true`)
* **Modifier Order:** `public,private,protected,internal,file,const,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,required,volatile,async`

## Documentation and Structure

* create comprehensive XML documentation for all public types and members
* include parameter descriptions and return value descriptions where applicable in XML comments
* comments inside methods should be used sparingly, only when the code is not self-explanatory explaning the **why**, not the **what**
* `true` and `false` in XML documentation are to be surrounded by `<c>`tags 
* do not document members that already have a <inheritdoc /> tag in their doc
* interfaces do not need to specify that they interfaces in the documentation, just document the purpose so that the doc can be inherited in implementations

## Testing instructions

* tests are located in the src/Net.Belt.Tests folder
* usually, one test class per production class
* test classes are named as the production class with the suffix "Tester"
* test methods are named using the pattern: MethodName_StateUnderTest_ExpectedBehavior
* tests should follow the Arrange-Act-Assert (AAA) pattern for clarity
* being a library, code coverage should be as high as possible
* test both positive and negative scenarios
* tests should be able to run in any order or in parallel without affecting each other
* support types in tests are to be placed in a separate sub-folder named "Support" within the test project with _internal_ accessibility
* minimize the logic in tests, specially avoid branching logic

### Frameworks
* tests are written using latest version of NUnit
* NUnit _Constraint Model_ is to be used for assertions, using assertion chaining when it improves readability
* test doubles can be created "manually" without the use of mocking frameworks, unless the complexity of the double is high enough to justify the use of a mocking framework
* the mocking framework to be used is NSubstitute
* parametrized tests can be used to reduce code duplication when testing multiple input scenarios for similar logic

### Test-First
* when asked to do so: write or update tests first, and then write the production code to make the tests pass

## Code quality
* avoid duplication of code
* keep methods small and focused on a single task, following Clean Code principles
* keep names consistent
* prefer string interpolation over string concatenation or fomatting
* implement disposable pattern correctly when dealing with unmanaged resources
* declare arguments are non-nullable by default, using nullable reference types where applicable
* use `is null` or `is not null` for null checks instead of direct comparisons to `null`
* trust types null annotations and don't add null checking when the type system already guarantees non-nullability

## Security considerations

## Commit messages
* use conventional commits style for commit messages
* icons can be used at the start of commit messages to improve readability, but are not mandatory

## Guardrails
* avoid introducing new dependencies unless absolutely necessary
* ask before introducing new dependencies
* ask before making large architectural changes
* ask before considering making a commit
