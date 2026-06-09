using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTest
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void Init_IsNewDefect()
        {
            var item = new Bug("A");
            Assert.AreEqual(BugState.NewDefect, item.CurrentState);
        }

        [TestMethod]
        public void Assign_GoesToTriage()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            Assert.AreEqual(BugState.Triage, item.CurrentState);
        }

        [TestMethod]
        public void StartFix_GoesToFixing()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            Assert.AreEqual(BugState.Fixing, item.CurrentState);
        }

        [TestMethod]
        public void FixDone_GoesToTesting()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.FixDone);
            Assert.AreEqual(BugState.Testing, item.CurrentState);
        }

        [TestMethod]
        public void TestPass_GoesToClosed()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.FixDone);
            item.Fire(BugTrigger.TestPass);
            Assert.AreEqual(BugState.Closed, item.CurrentState);
        }

        [TestMethod]
        public void TestFail_GoesToFixing()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.FixDone);
            item.Fire(BugTrigger.TestFail);
            Assert.AreEqual(BugState.Fixing, item.CurrentState);
        }

        [TestMethod]
        public void Reopen_GoesToReopened()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.FixDone);
            item.Fire(BugTrigger.TestPass);
            item.Fire(BugTrigger.Reopen);
            Assert.AreEqual(BugState.Reopened, item.CurrentState);
        }

        [TestMethod]
        public void ReopenedAssign_GoesToTriage()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.FixDone);
            item.Fire(BugTrigger.TestPass);
            item.Fire(BugTrigger.Reopen);
            item.Fire(BugTrigger.Assign);
            Assert.AreEqual(BugState.Triage, item.CurrentState);
        }

        [TestMethod]
        public void MarkDuplicate_GoesToClosed()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.MarkDuplicate);
            Assert.AreEqual(BugState.Closed, item.CurrentState);
        }

        [TestMethod]
        public void MarkNotBug_GoesToClosed()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.MarkNotBug);
            Assert.AreEqual(BugState.Closed, item.CurrentState);
        }

        [TestMethod]
        public void MarkCannotReproduce_GoesToState()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.MarkCannotReproduce);
            Assert.AreEqual(BugState.CannotReproduce, item.CurrentState);
        }

        [TestMethod]
        public void CR_OK_GoesToClosed()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.MarkCannotReproduce);
            item.Fire(BugTrigger.CR_OK);
            Assert.AreEqual(BugState.Closed, item.CurrentState);
        }

        [TestMethod]
        public void CR_Fail_GoesToReturned()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.MarkCannotReproduce);
            item.Fire(BugTrigger.CR_Fail);
            Assert.AreEqual(BugState.Returned, item.CurrentState);
        }

        [TestMethod]
        public void NoTime_GoesToDeferred()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.NoTime);
            Assert.AreEqual(BugState.Deferred, item.CurrentState);
        }

        [TestMethod]
        public void DeferredAssign_GoesToTriage()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.StartFix);
            item.Fire(BugTrigger.NoTime);
            item.Fire(BugTrigger.Assign);
            Assert.AreEqual(BugState.Triage, item.CurrentState);
        }

        [TestMethod]
        public void CanFire_Valid_ReturnsTrue()
        {
            var item = new Bug("A");
            Assert.IsTrue(item.CanFire(BugTrigger.Assign));
        }

        [TestMethod]
        public void CanFire_Invalid_ReturnsFalse()
        {
            var item = new Bug("A");
            Assert.IsFalse(item.CanFire(BugTrigger.StartFix));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_StartFix_FromNew_Throws()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_FixDone_FromTriage_Throws()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Assign);
            item.Fire(BugTrigger.FixDone);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invalid_Reopen_FromNew_Throws()
        {
            var item = new Bug("A");
            item.Fire(BugTrigger.Reopen);
        }

        [TestMethod]
        public void Props_AreSet()
        {
            var item = new Bug("X", "Y");
            Assert.AreEqual("X", item.Title);
            Assert.AreEqual("Y", item.Description);
        }
    }
}