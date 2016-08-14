﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal sealed class BlockBinder : LocalScopeBinder
    {
        private readonly BlockSyntax _block;

        public BlockBinder(Binder enclosing, BlockSyntax block)
            : this(enclosing, block, enclosing.Flags)
        {
        }

        public BlockBinder(Binder enclosing, BlockSyntax block, BinderFlags additionalFlags)
            : base(enclosing, enclosing.Flags | additionalFlags)
        {
            _block = block;
        }

        protected override ImmutableArray<LocalSymbol> BuildLocals()
        {
            return BuildLocals(_block.Statements, this);
        }

        protected override ImmutableArray<LocalFunctionSymbol> BuildLocalFunctions()
        {
            return BuildLocalFunctions(_block.Statements);
        }

        internal override bool IsLocalFunctionsScopeBinder
        {
            get
            {
                return true;
            }
        }

        protected override ImmutableArray<LabelSymbol> BuildLabels()
        {
            ArrayBuilder<LabelSymbol> labels = null;
            base.BuildLabels(_block.Statements, ref labels);
            return (labels != null) ? labels.ToImmutableAndFree() : ImmutableArray<LabelSymbol>.Empty;
        }

        internal override bool IsLabelsScopeBinder
        {
            get
            {
                return true;
            }
        }

        internal override ImmutableArray<LocalSymbol> GetDeclaredLocalsForScope(CSharpSyntaxNode scopeDesignator)
        {
            if (ScopeDesignator == scopeDesignator)
            {
                return this.Locals;
            }

            throw ExceptionUtilities.Unreachable;
        }

        internal override SyntaxNode ScopeDesignator
        {
            get
            {
                return _block;
            }
        }

        internal override ImmutableArray<LocalFunctionSymbol> GetDeclaredLocalFunctionsForScope(CSharpSyntaxNode scopeDesignator)
        {
            if (ScopeDesignator == scopeDesignator)
            {
                return this.LocalFunctions;
            }

            throw ExceptionUtilities.Unreachable;
        }
    }
}
