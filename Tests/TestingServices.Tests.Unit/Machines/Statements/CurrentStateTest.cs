﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using System;

using Microsoft.PSharp.Utilities;

using Xunit;

namespace Microsoft.PSharp.TestingServices.Tests.Unit
{
    public class CurrentStateTest : BaseTest
    {
        class Unit : Event { }

        class Server : Machine
        {
            [Start]
            [OnEntry(nameof(InitOnEntry))]
            [OnEventGotoState(typeof(Unit), typeof(Active))]
            class Init : MachineState { }

            void InitOnEntry()
            {
                this.Assert(this.CurrentState == typeof(Init));
                this.Raise(new Unit());
            }

            [OnEntry(nameof(ActiveOnEntry))]
            class Active : MachineState { }

            void ActiveOnEntry()
            {
                this.Assert(this.CurrentState == typeof(Active));
            }
        }

        /// <summary>
        /// P# semantics test: current state must be of the expected type.
        /// </summary>
        [Fact]
        public void TestCurrentState()
        {
            var configuration = base.GetConfiguration();
            configuration.SchedulingStrategy = SchedulingStrategy.DFS;

            var test = new Action<PSharpRuntime>((r) => {
                r.CreateMachine(typeof(Server));
            });

            base.AssertSucceeded(configuration, test);
        }
    }
}
