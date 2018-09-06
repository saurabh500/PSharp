﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using Xunit;

namespace Microsoft.PSharp.StaticAnalysis.Tests.Unit
{
    public class AccessBeforeSendTests : BaseTest
    {
        [Fact]
        public void TestAccessBeforeSend()
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
  this.Target = this.CreateMachine(typeof(M));
  letter.Text = ""changed"";
  this.Send(this.Target, new eUnit(letter));
 }
}
}";
            base.AssertSucceeded(test, isPSharpProgram: false);
        }

        [Fact]
        public void TestAccessBeforeSendInCallee()
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
  this.Target = this.CreateMachine(typeof(M));
  this.Foo(letter);
 }

 void Foo(Letter letter)
 {
  letter.Text = ""changed"";
  this.Send(this.Target, new eUnit(letter));
 }
}
}";
            base.AssertSucceeded(test, isPSharpProgram: false);
        }
    }
}
