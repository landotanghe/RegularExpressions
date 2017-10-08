using FiniteAutomota.NonDeterministic;
using System.Linq;

namespace RegularExpressionEvaluator
{
    public class RegularExpression
    {
        private Automaton<string, char> _automaton;
        private Sequence CompleteSequence;
        
        internal RegularExpression(Automaton<string, char> automaton, Sequence sequence)
        {
            _automaton = automaton;
            CompleteSequence = sequence;
        }
        
        public static RegularExpression For(string pattern)
        {
            var regularExpressionBuilder = new RegularExpressionBuilder(pattern);
            return regularExpressionBuilder.Build();
        }
        
        public bool IsMatch(string text)
        {
            //TODO add reset method to automaton
            foreach(var symbol in text)
            {
                _automaton.Process(symbol);
            }

            return _automaton.GetActiveStates().Any(state => state.Description == CompleteSequence.EndState);
        }
    }
}
