﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using Xunit;

namespace Microsoft.PSharp.StaticAnalysis.Tests.Unit
{
    public class AccessBeforeCreateMachineTests : BaseTest
    {
        #region correct tests

        [Fact]
        public void TestAccessBeforeCreateMachine()
        {
            var test = @"
using Microsoft.PSharp;

namespace Foo {
class eUnit : Event
{
 public Letter Letter;
 
 public eUnit(Letter letter)
  : base()
 {
  this.Letter = letter;
 }
}

struct Letter
{
 public string Text;

 public Letter(string text)
 {
  this.Text = text;
 }
}

class M : Machine
{
 MachineId Target;

 [Start]
 [OnEntry(nameof(FirstOnEntryAction))]
 class First : MachineState { }

 void FirstOnEntryAction()
 {
  var letter = new Letter(""test"");
  letter.Text = ""changed"";
  this.Target = this.CreateMachine(typeof(M), new eUnit(letter));
 }
}
}";
            base.AssertSucceeded(test, isPSharpProgram: false);
        }

        [Fact]
        public void TestAccessBeforeCreateMachineInCallee()
        {
            var test = @"
using Microsoft.PSharp;

namespace Foo {
class eUnit : Event
{
 public Letter Letter;
 
 public eUnit(Letter letter)
  : base()
 {
  this.Letter = letter;
 }
}

struct Letter
{
 public string Text;

 public Letter(string text)
 {
  this.Text = text;
 }
}

class M : Machine
{
 MachineId Target;

 [Start]
 [OnEntry(nameof(FirstOnEntryAction))]
 class First : MachineState { }

 void FirstOnEntryAction()
 {
  var letter = new Letter(""test"");
  this.Foo(letter);
 }

 void Foo(Letter letter)
 {
  letter.Text = ""changed"";
  this.Target = this.CreateMachine(typeof(M), new eUnit(letter));
 }
}
}";
            base.AssertSucceeded(test, isPSharpProgram: false);
        }

        #endregion
    }
}
