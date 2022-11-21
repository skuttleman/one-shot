using System;
using System.Collections.Generic;

namespace Game.System.FSM {
    public interface IFSM<State, Signal> {
        public bool CanTransition(State state, Signal signal);
        public FSMDetails<State, Signal> GetTransition(State state, Signal signal);
        public IFSM<State, Signal> AddTransition(State state, Signal signal, FSMDetails<State, Signal> details);
        public ISet<Signal> Signals(State state);
        public State Apply(State state, Signal signal);
    }

    public struct FSMDetails<State, Signal> {
        public readonly Action<State, Signal> before;
        public readonly Action<State> after;
        public readonly Func<State, Signal, bool> guard;
        public readonly State nextState;

        public FSMDetails(
            Action<State, Signal> before,
            Action<State> after,
            Func<State, Signal, bool> guard,
            State nextState
            ) {
            this.before = before;
            this.after = after;
            this.guard = guard;
            this.nextState = nextState;
        }

        public override bool Equals(object obj) {
            return obj is FSMDetails<State, Signal> details &&
                   EqualityComparer<Action<State, Signal>>.Default.Equals(before, details.before) &&
                   EqualityComparer<Action<State>>.Default.Equals(after, details.after) &&
                   EqualityComparer<Func<State, Signal, bool>>.Default.Equals(guard, details.guard);
        }

        public override int GetHashCode() {
            return HashCode.Combine(before, after, guard);
        }

        public static bool operator ==(FSMDetails<State, Signal> fsm1, FSMDetails<State, Signal> fsm2) =>
            fsm1.Equals(fsm2);
        public static bool operator !=(FSMDetails<State, Signal> fsm1, FSMDetails<State, Signal> fsm2) =>
            !fsm1.Equals(fsm2);
    }
    public class FSMInvalidStateTransitionException : Exception {
        public FSMInvalidStateTransitionException(string message) : base(message) { }
    }
}