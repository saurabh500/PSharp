﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.PSharp
{
    /// <summary>
    /// Factory for creating P# machines.
    /// </summary>
    internal static class MachineFactory
    {
        /// <summary>
        /// Cache storing machine constructors.
        /// </summary>
        private static Dictionary<Type, Func<Machine>> MachineConstructorCache;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MachineFactory()
        {
            MachineConstructorCache = new Dictionary<Type, Func<Machine>>();
        }

        /// <summary>
        /// Creates a new P# machine of the specified type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Machine</returns>
        public static Machine Create(Type type)
        {
            lock (MachineConstructorCache)
            {
                Func<Machine> constructor;
                if (!MachineConstructorCache.TryGetValue(type, out constructor))
                {
                    constructor = Expression.Lambda<Func<Machine>>(
                        Expression.New(type.GetConstructor(Type.EmptyTypes))).Compile();
                    MachineConstructorCache.Add(type, constructor);
                }

                return constructor();
            }
        }

        /// <summary>
        /// Checks if the constructor of the specified machine type exists in the cache.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Boolean</returns>
        internal static bool IsCached(Type type)
        {
            lock (MachineConstructorCache)
            {
                return MachineConstructorCache.ContainsKey(type);
            }
        }
    }
}
