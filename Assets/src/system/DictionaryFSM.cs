using System.Collections.Generic;
using Game.System.FSM;
using Game.Utils;

public class DictionaryFSM<State, Signal> : IFSM<State, Signal> {
    private Sequence<(State, Signal, FSMDetails<State, Signal>)> details;
    private IDictionary<State, ISet<Signal>> signals;

    public DictionaryFSM() {
        details = Sequence<(State, Signal, FSMDetails<State, Signal>)>.Empty();
        signals = new Dictionary<State, ISet<Signal>>();
    }

    public IFSM<State, Signal> AddTransition(State state, Signal signal, FSMDetails<State, Signal> detail) {
        Colls.Update(
            signals,
            state,
            sigs => sigs == null ? Sets.Of(signal) : Colls.Add(sigs, signal));
        details = Sequences.Cons((state, signal, detail), details);
        return this;
    }

    public bool CanTransition(State state, Signal signal) {
        ISet<Signal> sigs = Colls.Get(signals, state) ?? new HashSet<Signal>();
        return sigs.Contains(signal);
    }

    public FSMDetails<State, Signal> GetTransition(State state, Signal signal) =>
        Transitions(state, signal).First();
    public ISet<Signal> Signals(State state) => Colls.Get(signals, state);
    public State Apply(State state, Signal signal) =>
        ApplySignal(state, signal, Transitions(state, signal));

    private Sequence<FSMDetails<State, Signal>> Transitions(State state, Signal signal) =>
        details.Filter(tpl => tpl.Item1.Equals(state) && tpl.Item2.Equals(signal))
            .Map(tpl => tpl.Item3);

    private static State ApplySignal(State state, Signal signal, Sequence<FSMDetails<State, Signal>> details) {
        foreach (FSMDetails<State, Signal> detail in details) {
            if (detail.guard?.Invoke(state, signal) ?? true) {
                detail.before?.Invoke(state, signal);
                detail.after?.Invoke(detail.nextState);
                return detail.nextState;
            }
        }

        return state;
    }
}
