﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Structure.MetadataAsSource;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure.MetadataAsSource
{
    public class IndexerDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<IndexerDeclarationSyntax>
    {
        protected override string WorkspaceKind => CodeAnalysis.WorkspaceKind.MetadataAsSource;
        internal override AbstractSyntaxStructureProvider CreateProvider() => new MetadataIndexerDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
        public async Task NoCommentsOrAttributes()
        {
            const string code = @"
class Goo
{
    public string $$this[int x] { get; set; }
}";

            await VerifyNoBlockSpansAsync(code);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
        public async Task WithAttributes()
        {
            const string code = @"
class Goo
{
    {|hint:{|textspan:[Goo]
    |}public string $$this[int x] { get; set; }|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
        public async Task WithCommentsAndAttributes()
        {
            const string code = @"
class Goo
{
    {|hint:{|textspan:// Summary:
    //     This is a summary.
    [Goo]
    |}string $$this[int x] { get; set; }|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
        public async Task WithCommentsAttributesAndmodifiers()
        {
            const string code = @"
class Goo
{
    {|hint:{|textspan:// Summary:
    //     This is a summary.
    [Goo]
    |}public string $$this[int x] { get; set; }|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }
    }
}
