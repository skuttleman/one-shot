using System.Collections.Generic;
using Game.System.FSM;
using Game.Utils;

public class DictionaryFSM<State, Signal> : IFSM<State, Signal> {
    private static readonly string invalidTransitionFmt = "no valid transitions from {0} with signal {1}";
    private readonly IEnumerable<FSMDetails<State, Signal>> details;
    private readonly IDictionary<State, ISet<Signal>> signals;

    public DictionaryFSM(IEnumerable<FSMDetails<State, Signal>> details) {
        this.details = details;
        signals = details.Reduce(
            (acc, detail) => Colls.Update(
                acc,
                detail.state,
                set => Colls.Add(set, detail.signal),
                () => new HashSet<Signal>()),
            (IDictionary<State, ISet<Signal>>)new Dictionary<State, ISet<Signal>>());
    }

    public bool CanTransition(State state, Signal signal) {
        ISet<Signal> sigs = Colls.Get(signals, state) ?? new HashSet<Signal>();
        return sigs.Contains(signal);
    }

    public ISet<Signal> Signals(State state) => Colls.Get(signals, state);
    public State Apply(State state, Signal signal) {
        IEnumerable<FSMDetails<State, Signal>> transitions = details
            .Filter(detail => detail.state.Equals(state) && detail.signal.Equals(signal));
        return ApplySignal(state, signal, transitions);
    }

    public static State ApplySignal(
        State state,
        Signal signal,
        IEnumerable<FSMDetails<State, Signal>> details) {

        foreach (FSMDetails<State, Signal> detail in details) {
            if (detail.guard?.Invoke(state, signal) ?? true) {
                detail.before?.Invoke(state, signal);
                detail.after?.Invoke(detail.nextState);
                return detail.nextState;
            }
        }

        string message = string.Format(invalidTransitionFmt, state, signal);
        throw new FSMInvalidStateTransitionException(message);
    }
}
