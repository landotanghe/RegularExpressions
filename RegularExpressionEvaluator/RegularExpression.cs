using FiniteAutomota.NonDeterministic;

namespace RegularExpressionEvaluator
{
    public class RegularExpression
    {
        internal Automaton<string, char> _automaton;
        private Sequence CompleteSequence;
        
        internal RegularExpression(Sequence sequence)
        {
            _automaton = sequence.Builder.Build();
            CompleteSequence = sequence;
        }
        
        public static RegularExpression For(string pattern)
        {
            var regularExpressionBuilder = new RegularExpressionBuilder(pattern);
            return regularExpressionBuilder.Build();
        }
        
        public bool IsMatch(string text)
        {
            _automaton.Reset();
            foreach(var symbol in text)
            {
                _automaton.Process(symbol);
            }

            return _automaton.IsAccepted();
        }
    }
}
