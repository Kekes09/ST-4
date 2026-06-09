using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        NewDefect, Triage, Fixing, Testing, Closed, Reopened, Deferred,
        Duplicate, NotABug, CannotReproduce, Returned
    }

    public enum BugTrigger
    {
        Assign, StartFix, MarkDuplicate, MarkNotBug, MarkWontFix,
        MarkCannotReproduce, CR_OK, CR_Fail, NoTime, NeedInfo,
        FixDone, TestPass, TestFail, Reopen, ReturnTriage, Defer,
        NeedSolution, OtherProduct
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _stateMachine;
        public string Title { get; }
        public string Description { get; }
        public BugState CurrentState => _stateMachine.State;

        public Bug(string title, string description = "")
        {
            Title = title;
            Description = description;
            _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);

            _stateMachine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.Assign, BugState.Triage);

            _stateMachine.Configure(BugState.Triage)
                .Permit(BugTrigger.StartFix, BugState.Fixing)
                .Permit(BugTrigger.MarkDuplicate, BugState.Closed)
                .Permit(BugTrigger.MarkNotBug, BugState.Closed)
                .Permit(BugTrigger.MarkWontFix, BugState.Closed)
                .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce)
                .Permit(BugTrigger.Defer, BugState.Deferred);

            _stateMachine.Configure(BugState.Fixing)
                .Permit(BugTrigger.FixDone, BugState.Testing)
                .Permit(BugTrigger.NoTime, BugState.Deferred)
                .Permit(BugTrigger.NeedInfo, BugState.Triage)
                .Permit(BugTrigger.NeedSolution, BugState.Triage)
                .Permit(BugTrigger.OtherProduct, BugState.Triage);

            _stateMachine.Configure(BugState.Testing)
                .Permit(BugTrigger.TestPass, BugState.Closed)
                .Permit(BugTrigger.TestFail, BugState.Fixing);

            _stateMachine.Configure(BugState.CannotReproduce)
                .Permit(BugTrigger.CR_OK, BugState.Closed)
                .Permit(BugTrigger.CR_Fail, BugState.Returned);

            _stateMachine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            _stateMachine.Configure(BugState.Reopened)
                .Permit(BugTrigger.Assign, BugState.Triage)
                .Permit(BugTrigger.ReturnTriage, BugState.Triage);

            _stateMachine.Configure(BugState.Deferred)
                .Permit(BugTrigger.Assign, BugState.Triage);

            _stateMachine.Configure(BugState.Returned)
                .Permit(BugTrigger.Assign, BugState.Triage);
        }

        public void Fire(BugTrigger trigger) => _stateMachine.Fire(trigger);
        public bool CanFire(BugTrigger trigger) => _stateMachine.CanFire(trigger);
    }

    class Program
    {
        static void Main()
        {
            var item = new Bug("Sample", "Desc");
            Console.WriteLine($"Init: {item.CurrentState}");
            item.Fire(BugTrigger.Assign);
            Console.WriteLine($"Next: {item.CurrentState}");
        }
    }
}