﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

namespace Microsoft.CodeAnalysis.CSharp.DataFlowAnalysis
{
    /// <summary>
    /// Interface for a node.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Checks if the node contains the specified item.
        /// </summary>
        /// <returns>Boolean</returns>
        bool Contains<Item>(Item item);

        /// <summary>
        /// Checks if the node has no contents.
        /// </summary>
        /// <returns>Boolean</returns>
        bool IsEmpty();
    }
}
