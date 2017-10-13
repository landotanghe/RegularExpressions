using FiniteAutomota.NonDeterministic.Builder;
using RegularExpressionEvaluator.Builder;
using System;

namespace RegularExpressionEvaluator
{
    internal class RegularExpressionBuilder
    {
        private SubsequenceNamer SubsequenceNamer;
        private StateNamer StateNamer;
        private PatternReader PatternReader;

        public RegularExpressionBuilder(string pattern)
        {
            PatternReader = new PatternReader(pattern);
            StateNamer = new StateNamer();
            SubsequenceNamer = new SubsequenceNamer();
        }

        public RegularExpression Build()
        {
            var sequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());
            CreateStatesFor(sequence);
            return new RegularExpression(sequence);
        }

        private void CreateStatesFor(Sequence sequence)
        {
            var previous = sequence.StartState;
            var token = PatternReader.ReadNextToken();
            while (token.TokenType == TokenType.Character || token.TokenType == TokenType.StartNewSequence)
            {
                Sequence subsequence = CreateSubsequence(previous, token);
                subsequence = Repeat(subsequence);

                var subSequenceName = SubsequenceNamer.CreateNameForSubSequence();
                sequence.Builder.SubSequence(subsequence.Builder, subSequenceName)
                    .Transition().OnEpsilon().From(previous).To(subSequenceName);

                previous = subSequenceName;
                token = PatternReader.ReadNextToken();
            }

            if (token.TokenType != TokenType.OrOperator && token.TokenType != TokenType.EndNewSequence && token.TokenType != TokenType.EndOfInput)
                throw new Exception($"Token {token.TokenType} not expected here");

            if (token.TokenType == TokenType.OrOperator)
            {
                // Alternative path for same start and stop states
                CreateStatesFor(sequence);
            }

            sequence.Builder
                .Transition().OnEpsilon().From(previous).To(sequence.EndState);
        }

        private Sequence CreateSubsequence(string previousState, Token token)
        {
            Sequence subsequence;
            if (token.TokenType == TokenType.Character)
            {
                subsequence = CharacterSequence(token.Symbol);
            }
            else if (token.TokenType == TokenType.StartNewSequence)
            {
                subsequence = HandleNewSubSequence(previousState);
            }
            else
            {
                throw new NotImplementedException($"Token type {token.TokenType} can not be handled");
            }

            return subsequence;
        }

        private Sequence Repeat(Sequence sequenceToRepeat)
        {
            var nextToken = PatternReader.PeekNextToken();
            if (nextToken.TokenType == TokenType.OpenRepeat)
            {
                return RepeatPredefinedNumberOfTimes(sequenceToRepeat);
            }
            else if (nextToken.TokenType == TokenType.Repeat)
            {
                return RepeatAnyNumberOfTimes(sequenceToRepeat);
            }
            else
            {
                return sequenceToRepeat;
            }
        }

        private Sequence RepeatPredefinedNumberOfTimes(Sequence sequenceToRepeat)
        {
            var sequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());


            var repetitions = PatternReader.ReadRepetions();
            var previous = sequence.StartState;
            for (int i = 0; i < repetitions.Minimum; i++)
            {
                //TODO find out why it only works with the following 2 lines
               // sequenceToRepeat = new Sequence("test", "unit");
               // sequenceToRepeat.Builder.Transition().On('a').From("test").To("unit");

                var currentRepetition = SubsequenceNamer.CreateNameForSubSequence();
                sequence.Builder.SubSequence(sequenceToRepeat.Builder, currentRepetition)
                    .Transition().OnEpsilon().From(previous).To(currentRepetition);
                previous = currentRepetition;
            }
            for (int i = repetitions.Minimum; i < repetitions.Maximum; i++)
            {
                //TODO find out why it only works with the following 2 lines
                //sequenceToRepeat = new Sequence("test", "unit");
                //sequenceToRepeat.Builder.Transition().On('a').From("test").To("unit");

                var currentRepetition = SubsequenceNamer.CreateNameForSubSequence();
                sequence.Builder.SubSequence(sequenceToRepeat.Builder, currentRepetition)
                    .Transition().OnEpsilon().From(previous).To(currentRepetition)
                    .Transition().OnEpsilon().From(previous).To(sequence.EndState);
                previous = currentRepetition;
            }
            sequence.Builder.Transition().OnEpsilon().From(previous).To(sequence.EndState);
            
            return sequence;
        }

        private Sequence RepeatAnyNumberOfTimes(Sequence sequenceToRepeat)
        {
            var sequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());
            var subSequenceName = SubsequenceNamer.CreateNameForSubSequence();
            PatternReader.ReadNextToken();
            sequence.Builder.SubSequence(sequenceToRepeat.Builder, subSequenceName)
                .Transition().OnEpsilon().From(sequence.StartState).To(sequence.EndState)
                .Transition().OnEpsilon().From(sequence.EndState).To(subSequenceName)
                .Transition().OnEpsilon().From(subSequenceName).To(sequence.StartState);
            return sequence;
        }

        /// <summary>
        /// (Source)--symbol-->(Target)
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private Sequence CharacterSequence(char symbol)
        {
            var sequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());
            sequence.Builder.Transition().On(symbol).From(sequence.StartState).To(sequence.EndState);
            return sequence;
        }

        private Sequence HandleNewSubSequence(string previousState)
        {
            var subSequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());
            CreateStatesFor(subSequence);            

            var nextToken = PatternReader.PeekNextToken();
            if (nextToken.TokenType == TokenType.Repeat)
            {
                PatternReader.ReadNextToken();
                // Make loop to itself
                subSequence.Builder
                    .Transition().OnEpsilon().From(subSequence.EndState).To(subSequence.StartState)
                    .Transition().OnEpsilon().From(subSequence.StartState).To(subSequence.EndState);
            }

            return subSequence;
        }
    }
}
