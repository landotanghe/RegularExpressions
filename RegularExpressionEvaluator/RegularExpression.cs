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

        private class PatternReader
        {
            public const char EscapeChar = '\\';

            private int nextSymbolPosition = 0;
            private string _input;
            public PatternReader(string input)
            {
                _input = input;
            }

            public char ReadNextSymbol()
            {
                var nextSymbol = _input[nextSymbolPosition++];
                if (nextSymbol == EscapeChar)
                {
                    nextSymbol = _input[nextSymbolPosition++];
                    if (nextSymbol == 't')
                        nextSymbol = '\t';
                }
                return nextSymbol;
            }

            public bool HasUnprocessedInput()
            {
                return nextSymbolPosition < _input.Length;
            }
        }

        public RegularExpression(string pattern)
        {
            var automatonBuilder = new AutomatonBuilder();
            var stateId = 0;
            
            automatonBuilder
                .State(StartState).ActiveAtStart();

            var reader = new PatternReader(pattern);
            
            var previousState = StartState;
            while(reader.HasUnprocessedInput())
            {
                var symbol = reader.ReadNextSymbol();

                var currentState = "Character " + stateId++;

                automatonBuilder.State(currentState)
                    .Transition().On(symbol).From(previousState).To(currentState);

                previousState = currentState;
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
