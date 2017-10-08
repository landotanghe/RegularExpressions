using FiniteAutomota.NonDeterministic.Builder;
using System;

namespace RegularExpressionEvaluator
{
    internal class RegularExpressionBuilder
    {

        private StateNamer StateNamer;
        private PatternReader PatternReader;
        private AutomatonBuilder AutomatonBuilder;

        public RegularExpressionBuilder(string pattern)
        {
            PatternReader = new PatternReader(pattern);
            AutomatonBuilder = new AutomatonBuilder();
            StateNamer = new StateNamer();
        }

        public RegularExpression Build()
        {
            var sequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());

            AutomatonBuilder.State(sequence.StartState).ActiveAtStart();
            AutomatonBuilder.State(sequence.EndState);

            CreateStatesFor(sequence);
            var automaton = AutomatonBuilder.Build();
            return new RegularExpression(automaton, sequence);
        }

        private void CreateStatesFor(Sequence sequence)
        {
            var previousState = sequence.StartState;
            var token = PatternReader.ReadNextToken();
            while (token.TokenType == TokenType.Character || token.TokenType == TokenType.StartNewSequence)
            {
                if (token.TokenType == TokenType.Character)
                {
                    var currentState = StateNamer.CreateNameForIntermediateState();

                    AutomatonBuilder.State(currentState)
                        .Transition().On(token.Symbol).From(previousState).To(currentState);

                    previousState = currentState;
                }
                else if (token.TokenType == TokenType.StartNewSequence)
                {
                    var subSequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());

                    AutomatonBuilder
                        .State(subSequence.StartState)
                        .State(subSequence.EndState)
                        .Transition().OnEpsilon().From(previousState).To(subSequence.StartState);

                    CreateStatesFor(subSequence);
                    previousState = subSequence.EndState;
                }
                else
                {
                    throw new NotImplementedException($"Token type {token.TokenType} can not be handled");
                }
                token = PatternReader.ReadNextToken();
            }

            if (token.TokenType == TokenType.OrOperator)
            {
                // To create an or operator, simply add an alternative path for same start and stop states
                CreateStatesFor(sequence);
            }

            AutomatonBuilder
                .Transition().OnEpsilon().From(previousState).To(sequence.EndState);
        }
    }
}
