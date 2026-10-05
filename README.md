# gramide-csharp

An independent C# **core source reader** for gramide, version 0.1.0.
The package depends only on gramide v0.2.11. It ships a byte-preserving
lexer, a recursive grammar, a committed compiled table, symbol rules, an
independent CLI and a small positive/negative regression corpus.

This is not a complete C# parser or a compiler validation gate. It advertises
`tokens`, `parse`, `outline`, `symbols`, `tags`, and `map`, and deliberately
omits `check` and `symbols-recovered`. `symbols` requires a strict parse.
The host's `parse` and `outline` can recover damaged input with diagnostics
and ERROR nodes. A recovered tree does not establish that C# source is valid.

## Covered core

- Block/file-scoped namespaces; ordinary, alias, static and global using
  directives; classes, structs, interfaces, enums, records and delegates
- Nested declarations, modifiers/attributes, generic parameters and variance,
  where constraints, qualified/alias-qualified generic types, nullable and
  array types, base lists and positional/primary constructor parameter lists
- Fields, methods and generic methods, constructors and base/this initializer
  calls, properties/accessors, expression-bodied methods/properties, events
  with accessors, enum constants and multiple field declarators
- Local declarations, blocks, if/else, while/do/for/foreach, return/throw,
  break/continue/goto, old-style switch labels, try/catch filters/finally,
  using, lock, checked/unchecked, yield and await expressions
- Precedence-aware arithmetic/boolean/bitwise/comparison expressions,
  assignments, ternaries, null coalescing, casts, lambdas, calls and named/
  ref/out/in arguments, member/index access, null-conditional member access,
  object/array construction and common object/collection initializers
- UTF-8 names including verbatim `@identifiers`, original byte ranges,
  non-nesting comments, regular/character/verbatim string literals and common
  decimal, binary and hexadecimal number spellings/suffixes

## Deliberate limits

- Preprocessor directives, escaped Unicode identifiers, interpolated strings,
  raw strings, raw interpolated strings and UTF-8 string suffixes are not
  supported. These are not hidden inside generic literal/body token buckets
- Identifier categories are pinned to Unicode **15.0.0**, generated from
  Python's Unicode database. Characters introduced later are unsupported;
  no Roslyn Unicode-version equivalence is claimed
- Top-level statements, pattern matching beyond a simple `is Type`, switch
  expressions, tuples/deconstruction, query syntax, extension blocks,
  collection expressions, ranges, index-from-end, target-typed `new`,
  anonymous object creation, local functions, explicit-interface members,
  indexer/operator/conversion declarations, destructors, function pointers,
  unsafe pointer expressions and fixed buffers are outside this core
- Array types may be multidimensional; new-array dimension syntax currently
  supports single expressions per bracket. Type-ref return forms, contextual
  `field` semantics and attributes with keyword targets are not fully modeled
- Syntax-oriented reading does not check modifier combinations, declaration
  placement, assignment targets, type availability, constructor-name equality,
  legal constant values, numeric overflow or Unicode escape scalar values
- Numeric separators immediately after a base prefix are not implemented.
  `>` is split for nested generic arguments; assembled shifts and `>=` do not
  enforce byte adjacency. C# unsigned right shift is outside this core
- Symbols are lexical and namespace/type-qualified. Constructors use
  `Type.Type`; verbatim names retain their source `@` spelling. Field ranges
  inherit the declaration prefix and end at the individual declarator, before
  `;`. Record parameters do not synthesize property symbols. Accessors are
  represented in the tree under a property, not emitted as independent methods
- No Roslyn differential oracle or real-repository acceptance percentage is
  claimed. This is tested core coverage, not C# language-version conformance

## Build and verify

Run from this directory with Almide 0.62.0 or a compatible compiler:

```sh
almide build cli/main.almd -o gramide_csharp
./gramide_csharp symbols fixtures/valid/model.cs
./gramide_csharp outline fixtures/valid/statements.cs
./gramide_csharp gen-table > src/table.almd
ci/check.sh
```

`ci/check.sh` runs type checking and Almide tests, builds the independent CLI,
checks that table regeneration is deterministic, then runs `ci/verify.py`.
The fixture manifest asserts all expected symbol names; the verifier checks
UTF-8 byte and line ranges, exercises reader commands, rejects unsupported
capability claims, and requires malformed inputs to fail strict symbols with
no partial JSON. Almide tests compare the dynamic grammar with the shipped
table and cover scanner/literal failures. `ci/generate_unicode.py` reproduces
the category data with Python's Unicode 15.0.0 database.

## Sources

Original implementation informed by Microsoft's [C# language
specification](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/)
and [record reference](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record).
No third-party parser grammar was copied.

Unicode category facts in `src/unicode.almd` derive from the
[Unicode 15.0.0 Character Database](https://www.unicode.org/Public/15.0.0/ucd/UnicodeData.txt)
via Python 3.12 `unicodedata`. They are redistributed with the full
`LICENSE-UNICODE` notice. The generator refuses any different database version.

Source line endings are LF or CRLF. Lone CR is refused in strict mode because
the pinned engine uses LF-based document line counts; this prevents inconsistent
complete symbol ranges for older-Mac or control-bearing source files.

## Repository contract

This repository owns this language package and its tests. `src/mod.almd` exports
`definition()` using the shared gramide package API. The `gramide-cli` repository
composes it as a git dependency; no grammar source is vendored into the CLI.
`bash ci/check.sh` runs the complete package gate with an explicit test entry
point, avoiding recursive parallel compiler fan-out. CI pins Almide and Rust
in `.github/workflows/quality.yml`.
