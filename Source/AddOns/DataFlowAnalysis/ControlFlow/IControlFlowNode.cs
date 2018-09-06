﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.CSharp.DataFlowAnalysis
{
    /// <summary>
    /// Interface for a control-flow graph node.
    /// </summary>
    public interface IControlFlowNode : INode, ITraversable<IControlFlowNode>
    {
        /// <summary>
        /// List of statements contained in the node.
        /// </summary>
        IList<Statement> Statements { get; }

        /// <summary>
        /// Graph that contains this node.
        /// </summary>
        IGraph<IControlFlowNode> Graph { get; }

        /// <summary>
        /// Method summary that contains this node.
        /// </summary>
        MethodSummary Summary { get; }
    }
}
