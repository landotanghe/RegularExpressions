using FiniteAutomota.NonDeterministic;
using FiniteAutomota.NonDeterministic.Builder;
using System.Linq;

namespace RegularExpressionEvaluator
{
    public class RegularExpression
    {
        private Automaton<string, char> _automaton;

        private const string StartState = "Start";
        private const string FinalState = "Final";

        public RegularExpression(string pattern)
        {
            var automatonBuilder = new AutomatonBuilder();
            var stateId = 0;
            
            automatonBuilder
                .State(StartState).ActiveAtStart();

            var previousState = StartState;
            for (int i =0; i < pattern.Length; i++)
            {
                var input = pattern[i];

                var currentState = "Character " + stateId++;

                automatonBuilder.State(currentState)
                    .Transition().On(input).From(previousState).To(currentState);
            }

            automatonBuilder.State(FinalState)
                .Transition().OnEpsilon().From(previousState).To(FinalState);

            _automaton = automatonBuilder.Build();
        }

        public bool IsMatch(string text)
        {
            //TODO add reset method to automaton
            foreach(var symbol in text)
            {
                _automaton.Process(symbol);
            }

            return _automaton.GetActiveStates().Any(state => state.Description == FinalState);
        }
    }
}
