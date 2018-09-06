﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using System;

using Xunit;

namespace Microsoft.PSharp.TestingServices.Tests.Unit
{
    public class EntryPointThrowExceptionTest : BaseTest
    {
        class M : Machine
        {
            [Start]
            class Init : MachineState { }
        }

        [Fact]
        public void TestEntryPointThrowException()
        {
            var test = new Action<PSharpRuntime>((r) => {
                MachineId m = r.CreateMachine(typeof(M));
                throw new InvalidOperationException();
            });

            base.AssertFailedWithException(test, typeof(InvalidOperationException), true);
        }

        [Fact]
        public void TestEntryPointNoMachinesThrowException()
        {
            var test = new Action<PSharpRuntime>((r) => {
                throw new InvalidOperationException();
            });

            base.AssertFailedWithException(test, typeof(InvalidOperationException), true);
        }
    }
}
