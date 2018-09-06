﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.PSharp.StaticAnalysis
{
    /// <summary>
    /// Class implementing a call trace step.
    /// </summary>
    internal class CallTraceStep
    {
        /// <summary>
        /// The method declaration.
        /// </summary>
        internal readonly BaseMethodDeclarationSyntax Method;

        /// <summary>
        /// The invocation expression.
        /// </summary>
        internal readonly ExpressionSyntax Invocation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="invocation">Invocation</param>
        internal CallTraceStep(BaseMethodDeclarationSyntax method, ExpressionSyntax invocation)
        {
            this.Method = method;
            this.Invocation = invocation;
        }
    }
}
