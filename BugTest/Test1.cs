using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using Stateless;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitialState_IsNewDefect()
        {
            var bug = new Bug("Test Bug");
            Assert.AreEqual(State.NewDefect, bug.CurrentState);
        }

        [TestMethod]
        public void Assign_FromNewDefect_GoesToTriage()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            Assert.AreEqual(State.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void StartFix_FromTriage_GoesToFixing()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            Assert.AreEqual(State.Fixing, bug.CurrentState);
        }

        [TestMethod]
        public void NotADefect_FromTriage_GoesToNotABug()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.NotADefect);
            Assert.AreEqual(State.NotABug, bug.CurrentState);
        }

        [TestMethod]
        public void WontFix_FromTriage_GoesToClosed()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.WontFix);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Duplicate_FromTriage_GoesToDuplicate()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Duplicate);
            Assert.AreEqual(State.Duplicate, bug.CurrentState);
        }

        [TestMethod]
        public void Defer_FromTriage_GoesToDeferred()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Defer);
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void FixCompleted_FromFixing_GoesToTesting()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            Assert.AreEqual(State.Testing, bug.CurrentState);
        }

        [TestMethod]
        public void TestPassed_FromTesting_GoesToClosed()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            bug.Fire(Trigger.TestPassed);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void TestFailed_FromTesting_GoesToFixing()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            bug.Fire(Trigger.TestFailed);
            Assert.AreEqual(State.Fixing, bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_FromClosed_GoesToReopened()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            bug.Fire(Trigger.TestPassed);
            bug.Fire(Trigger.Reopen);
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void StartFix_FromReopened_GoesToFixing()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            bug.Fire(Trigger.TestPassed);
            bug.Fire(Trigger.Reopen);
            bug.Fire(Trigger.StartFix);
            Assert.AreEqual(State.Fixing, bug.CurrentState);
        }

        [TestMethod]
        public void NoTimeNow_FromFixing_GoesToDeferred()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.NoTimeNow);
            Assert.AreEqual(State.Deferred, bug.CurrentState);
        }

        [TestMethod]
        public void NeedsSeparateSolution_FromFixing_GoesToTriage()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.NeedsSeparateSolution);
            Assert.AreEqual(State.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void OtherProductProblem_FromFixing_GoesToTriage()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.OtherProductProblem);
            Assert.AreEqual(State.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void NeedMoreInfo_FromFixing_GoesToTriage()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.NeedMoreInfo);
            Assert.AreEqual(State.Triage, bug.CurrentState);
        }

        [TestMethod]
        public void CannotReproduce_FromTriage_GoesToCannotReproduce()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.CannotReproduce);
            Assert.AreEqual(State.CannotReproduce, bug.CurrentState);
        }

        [TestMethod]
        public void CannotReproduceOK_FromCannotReproduce_GoesToClosed()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.CannotReproduce);
            bug.Fire(Trigger.CannotReproduceOK);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void CannotReproduceNotOK_FromCannotReproduce_GoesToReturned()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.CannotReproduce);
            bug.Fire(Trigger.CannotReproduceNotOK);
            Assert.AreEqual(State.Returned, bug.CurrentState);
        }

        [TestMethod]
        public void Assign_FromDeferred_GoesToTriage()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Defer);
            bug.Fire(Trigger.Assign);
            Assert.AreEqual(State.Triage, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_StartFix_FromNewDefect_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_FixCompleted_FromNewDefect_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.FixCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_TestPassed_FromNewDefect_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.TestPassed);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Reopen_FromNewDefect_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Reopen);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Assign_FromTriage_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.Assign);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_StartFix_FromFixing_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_FixCompleted_FromClosed_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.WontFix);
            bug.Fire(Trigger.FixCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_TestFailed_FromNewDefect_ThrowsException()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.TestFailed);
        }

        [TestMethod]
        public void BugProperties_AreSetCorrectly()
        {
            var bug = new Bug("My Bug Title", "My Bug Description");
            Assert.AreEqual("My Bug Title", bug.Title);
            Assert.AreEqual("My Bug Description", bug.Description);
        }

        [TestMethod]
        public void CanFire_ReturnsTrueForValidTransition()
        {
            var bug = new Bug("Test Bug");
            Assert.IsTrue(bug.CanFire(Trigger.Assign));
        }

        [TestMethod]
        public void CanFire_ReturnsFalseForInvalidTransition()
        {
            var bug = new Bug("Test Bug");
            Assert.IsFalse(bug.CanFire(Trigger.StartFix));
        }

        [TestMethod]
        public void CompleteWorkflow_NewToClosed()
        {
            var bug = new Bug("Test Bug");
            Assert.AreEqual(State.NewDefect, bug.CurrentState);

            bug.Fire(Trigger.Assign);
            Assert.AreEqual(State.Triage, bug.CurrentState);

            bug.Fire(Trigger.StartFix);
            Assert.AreEqual(State.Fixing, bug.CurrentState);

            bug.Fire(Trigger.FixCompleted);
            Assert.AreEqual(State.Testing, bug.CurrentState);

            bug.Fire(Trigger.TestPassed);
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void ReopenWorkflow_ClosedToReopenedToFixing()
        {
            var bug = new Bug("Test Bug");
            bug.Fire(Trigger.Assign);
            bug.Fire(Trigger.StartFix);
            bug.Fire(Trigger.FixCompleted);
            bug.Fire(Trigger.TestPassed);
            bug.Fire(Trigger.Reopen);
            bug.Fire(Trigger.StartFix);

            Assert.AreEqual(State.Fixing, bug.CurrentState);
        }
    }
}