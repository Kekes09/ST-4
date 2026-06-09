using System;
using Stateless;

namespace BugPro
{
    public enum State
    {
        NewDefect,
        Triage,
        Fixing,
        Testing,
        Closed,
        Reopened,
        Deferred,
        CannotReproduce,
        Returned
    }

    public enum Trigger
    {
        Assign,
        StartFix,
        Duplicate,
        NotABug,
        WontFix,
        CannotReproduce,
        CannotReproduceOk,
        CannotReproduceFail,
        NoTimeNow,
        NeedMoreInfo,
        FixCompleted,
        TestPassed,
        TestFailed,
        ProblemSolved,
        Reopen,
        
    }

    public class Bug
    {
        private readonly StateMachine<State, Trigger> _machine;

        public State CurrentState => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<State, Trigger>(State.NewDefect);

            _machine.Configure(State.NewDefect)
                .Permit(Trigger.Assign, State.Triage);

            _machine.Configure(State.Triage)
                .Permit(Trigger.StartFix, State.Fixing)
                .Permit(Trigger.Duplicate, State.Closed)
                .Permit(Trigger.NotABug, State.Closed)
                .Permit(Trigger.WontFix, State.Closed)
                .Permit(Trigger.CannotReproduce, State.CannotReproduce);

            _machine.Configure(State.Fixing)
                .Permit(Trigger.FixCompleted, State.Testing)
                .Permit(Trigger.NoTimeNow, State.Deferred)
                .Permit(Trigger.NeedMoreInfo, State.Triage);

            _machine.Configure(State.Testing)
                .Permit(Trigger.TestPassed, State.Closed)
                .Permit(Trigger.TestFailed, State.Fixing)
                .Permit(Trigger.ProblemSolved, State.Reopened);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.Assign, State.Triage);

            _machine.Configure(State.Deferred)
                .Permit(Trigger.Assign, State.Triage);

            _machine.Configure(State.CannotReproduce)
                .Permit(Trigger.CannotReproduceOk, State.Closed)
                .Permit(Trigger.CannotReproduceFail, State.Returned);

            _machine.Configure(State.Returned)
                .Permit(Trigger.Assign, State.Triage);
        }

        public void Fire(Trigger trigger)
        {
            _machine.Fire(trigger);
        }

        public bool CanFire(Trigger trigger)
        {
            return _machine.CanFire(trigger);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Trigger.Assign);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Trigger.StartFix);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Trigger.FixCompleted);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Trigger.TestPassed);
            Console.WriteLine($"State: {bug.CurrentState}");
        }
    }
}