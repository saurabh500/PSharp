﻿//-----------------------------------------------------------------------
// <copyright file="StateGroupFailTests.cs">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// 
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//      EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//      MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//      IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//      CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//      TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//      SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.PSharp.LanguageServices.Parsing;

namespace Microsoft.PSharp.LanguageServices.Tests.Unit
{
    [TestClass]
    public class StateGroupFailTests
    {
        [TestMethod, Timeout(10000)]
        public void TestMachineStateDeclarationWithMoreThanOneEntry()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
entry {}
entry{}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Duplicate entry declaration.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineStateDeclarationWithMoreThanOneExit()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
exit{}
exit {}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Duplicate exit declaration.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineEntryDeclarationWithUnexpectedIdentifier()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
entry Bar {}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \"{\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineOnEventGotoStateDeclarationWithoutSemicolon()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S1
{
on e goto S2
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \";\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineOnEventGotoStateDeclarationWithoutState()
        {
            var test = @"
namespace Foo {
machine M {
group G 
{
start state S1
{
on e goto;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected state identifier.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineOnEventDoActionDeclarationWithoutSemicolon()
        {
            var test = @"
namespace Foo {
machine M {
start state S1
{
on e do Bar
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \";\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineOnEventDoActionDeclarationWithoutAction()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S1
{
on e do;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected action identifier.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineOnEventDeclarationWithoutHandler()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S1
{
on e;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \"do\", \"goto\" or \"push\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineIgnoreEventDeclarationWithoutComma()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
ignore e1 e2;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \",\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineIgnoreEventDeclarationWithExtraComma()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
ignore e1,e2,;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected event identifier.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineDeferEventDeclarationWithoutComma()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
defer e1 e2;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \",\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineDeferEventDeclarationWithExtraComma()
        {
            var test = @"
namespace Foo {
machine M {
group G {
start state S
{
defer e1,e2,;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected event identifier.", test);
        }
        
        [TestMethod, Timeout(10000)]
        public void TestMachineGroupInsideState()
        {
            var test = @"
namespace Foo {
machine M {
start state S
{
group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Unexpected token.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineEmptyGroup()
        {
            var test = @"
namespace Foo {
machine M {
group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A state group must declare at least one state.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineEmptyNestedGroup()
        {
            var test = @"
namespace Foo {
machine M {
group G {
group G2 { }
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A state group must declare at least one state.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineMethodInsideGroup()
        {
            var test = @"
namespace Foo {
machine M {
group G {
void Bar() { }
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Unexpected token 'void'.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineStartGroup()
        {
            var test = @"
namespace Foo {
machine M {
start group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be marked start.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineColdGroup()
        {
            var test = @"
namespace Foo {
machine M {
cold group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be cold.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineHotGroup()
        {
            var test = @"
namespace Foo {
machine M {
hot group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be hot.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMachineGroupName()
        {
            var test = @"
namespace Foo {
machine M {
group G.G2 { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \"{\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorStateDeclarationWithMoreThanOneEntry()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S
{
entry {}
entry{}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Duplicate entry declaration.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorStateDeclarationWithMoreThanOneExit()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S
{
exit{}
exit {}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Duplicate exit declaration.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorEntryDeclarationWithUnexpectedIdentifier()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S
{
entry Bar {}
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \"{\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorOnEventGotoStateDeclarationWithoutSemicolon()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S1
{
on e goto S2
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \";\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorOnEventGotoStateDeclarationWithoutState()
        {
            var test = @"
namespace Foo {
monitor M {
group G 
{
start state S1
{
on e goto;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected state identifier.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorOnEventDoActionDeclarationWithoutSemicolon()
        {
            var test = @"
namespace Foo {
monitor M {
start state S1
{
on e do Bar
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \";\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorOnEventDoActionDeclarationWithoutAction()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S1
{
on e do;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected action identifier.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorOnEventDeclarationWithoutHandler()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S1
{
on e;
}
}
}
}";

            LanguageTestUtilities.AssertFailedTestLog("Expected \"do\", \"goto\" or \"push\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorIgnoreEventDeclarationWithoutComma()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S
{
ignore e1 e2;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \",\".", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorIgnoreEventDeclarationWithExtraComma()
        {
            var test = @"
namespace Foo {
monitor M {
group G {
start state S
{
ignore e1,e2,;
}
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected event identifier.", test);
        }
        
        [TestMethod, Timeout(10000)]
        public void TestMonitorGroupInsideState()
        {
            var test = @"
namespace Foo {
monitor M {
start state S
{
group G { }
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Unexpected token.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorEmptyGroup()
        {
            var test = @"
namespace Foo {
monitor M {
group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A state group must declare at least one state.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorEmptyNestedGroup()
        {
            var test = @"
namespace Foo {
monitor M {
group G
{
group G2 { }
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A state group must declare at least one state.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorMethodInsideGroup()
        {
            var test = @"
namespace Foo {
monitor M {
group G
{
void Bar() { }
}
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Unexpected token 'void'.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorStartGroup()
        {
            var test = @"
namespace Foo {
monitor M {
start group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be marked start.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorColdGroup()
        {
            var test = @"
namespace Foo {
monitor M {
cold group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be cold.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorHotGroup()
        {
            var test = @"
namespace Foo {
monitor M {
hot group G { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("A machine state group cannot be hot.", test);
        }

        [TestMethod, Timeout(10000)]
        public void TestMonitorGroupName()
        {
            var test = @"
namespace Foo {
monitor M {
group G.G2 { }
}
}";
            LanguageTestUtilities.AssertFailedTestLog("Expected \"{\".", test);
        }
    }
}
