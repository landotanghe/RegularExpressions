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

        internal RegularExpression(Automaton<string, char> automaton)
        {
            _automaton = automaton;
        }

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
        public static RegularExpression For(string pattern)
        {
            var regularExpressionBuilder = new RegularExpressionBuilder(pattern);
            return regularExpressionBuilder.Build();
        }

        internal class RegularExpressionBuilder
        {
            private PatternReader PatternReader;

            public RegularExpressionBuilder(string pattern)
            {
                PatternReader = new PatternReader(pattern);
            }            

            public RegularExpression Build()
            {
                var automatonBuilder = new AutomatonBuilder();
                var stateId = 0;

                automatonBuilder
                    .State(StartState).ActiveAtStart();
                
                var previousState = StartState;
                while (PatternReader.HasUnprocessedInput())
                {
                    var symbol = PatternReader.ReadNextSymbol();

                    var currentState = "Character " + stateId++;

                    automatonBuilder.State(currentState)
                        .Transition().On(symbol).From(previousState).To(currentState);

                    previousState = currentState;
                }

                automatonBuilder.State(FinalState)
                    .Transition().OnEpsilon().From(previousState).To(FinalState);

                var automaton = automatonBuilder.Build();
                return new RegularExpression(automaton);
            }
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
