﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using Xunit;

namespace Microsoft.PSharp.StaticAnalysis.Tests.Unit
{
    public class NoStatementsWithLoopTest : BaseTest
    {
        #region correct tests

        [Fact]
        public void TestNoStatementsWithLoop()
        {
            var test = @"
using Microsoft.PSharp;

namespace Foo {
class M : Machine
{
 [Start]
 [OnEntry(nameof(FirstOnEntryAction))]
 class First : MachineState { }

 void FirstOnEntryAction()
 {
  for (int i = 0; i < 2; i++) { }
 }
}
}";
            base.AssertSucceeded(test, isPSharpProgram: false);
        }

        #endregion
    }
}
