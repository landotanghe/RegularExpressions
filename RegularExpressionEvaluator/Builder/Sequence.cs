using FiniteAutomota.NonDeterministic.Builder;

namespace RegularExpressionEvaluator
{
    internal class Sequence
    {
        public Sequence(string startState, string endState)
        {
            StartState = startState;
            EndState = endState;
            Builder = new AutomatonBuilder();
            Builder.State(startState).ActiveAtStart()
                .State(endState).Final();
        }

        public AutomatonBuilder Builder { get; }

        public string StartState { get; }
        public string EndState { get; }
    }
}
