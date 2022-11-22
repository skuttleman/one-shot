using System;
using System.Collections.Generic;

namespace Game.System.FSM {
    public interface IFSM<State, Signal> {
        public bool CanTransition(State state, Signal signal);
        public ISet<Signal> Signals(State state);
        public State Apply(State state, Signal signal);
    }

    public record FSMDetails<State, Signal>(
            Action<State, Signal> before,
            Action<State> after,
            Func<State, Signal, bool> guard,
            State state,
            Signal signal,
            State nextState);

    public class FSMInvalidStateTransitionException : Exception {
        public FSMInvalidStateTransitionException(string message) : base(message) { }
    }
}