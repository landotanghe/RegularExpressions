﻿using FiniteAutomota.NonDeterministic.Builder;
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
            AutomatonBuilder.State(sequence.EndState).Final();

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
                    previousState = HandleCharacter(previousState, token.Symbol);
                }
                else if (token.TokenType == TokenType.StartNewSequence)
                {
                    previousState = HandleNewSubSequence(previousState);
                }
                else
                {
                    throw new NotImplementedException($"Token type {token.TokenType} can not be handled");
                }
                token = PatternReader.ReadNextToken();
            }

            if (token.TokenType != TokenType.OrOperator && token.TokenType != TokenType.EndNewSequence && token.TokenType != TokenType.EndOfInput)
                throw new Exception($"Token {token.TokenType} not expected here");

            if (token.TokenType == TokenType.OrOperator)
            {
                // Alternative path for same start and stop states
                CreateStatesFor(sequence);
            }

            AutomatonBuilder
                .Transition().OnEpsilon().From(previousState).To(sequence.EndState);
        }

        private string HandleCharacter(string previousState, char symbol)
        {
            var currentState = StateNamer.CreateNameForIntermediateState();

            var nextToken = PatternReader.PeekNextToken();
            if (nextToken.TokenType == TokenType.OpenRepeat)
            {
                var sequenceToRepeat = new Sequence(currentState, currentState);
                var x = sequenceToRepeat.StartState;

                var repetitions = PatternReader.ReadRepetions();
                for(int i = 0; i < repetitions.Minimum; i++)
                {
                    AutomatonBuilder.State(x)
                        .Transition().On(symbol).From(previousState).To(x);
                    previousState = x;
                    x = StateNamer.CreateNameForIntermediateState();
                }
                for(int i = repetitions.Minimum; i < repetitions.Maximum; i++)
                {
                    AutomatonBuilder.State(x)
                        .Transition().OnEpsilon().From(previousState).To(x)
                        .Transition().On(symbol).From(previousState).To(x);

                    previousState = x;
                    x = StateNamer.CreateNameForIntermediateState();
                }

                currentState = previousState;
            }
            else if (nextToken.TokenType == TokenType.Repeat)
            {
                PatternReader.ReadNextToken();
                AutomatonBuilder.State(currentState)
                    .Transition().OnEpsilon().From(previousState).To(currentState)
                    .Transition().On(symbol).From(currentState).To(currentState);
            }
            else
            {
                AutomatonBuilder.State(currentState)
                    .Transition().On(symbol).From(previousState).To(currentState);
            }
            return currentState;
        }

        private string HandleNewSubSequence(string previousState)
        {
            var subSequence = new Sequence(StateNamer.CreateNameForStartOfSequence(), StateNamer.CreateNameForEndOfSequence());

            AutomatonBuilder
                .State(subSequence.StartState)
                .State(subSequence.EndState)
                .Transition().OnEpsilon().From(previousState).To(subSequence.StartState);

            CreateStatesFor(subSequence);
            

            var nextToken = PatternReader.PeekNextToken();
            if (nextToken.TokenType == TokenType.Repeat)
            {
                PatternReader.ReadNextToken();
                // Make loop to itself
                AutomatonBuilder
                    .Transition().OnEpsilon().From(subSequence.EndState).To(subSequence.StartState)
                    .Transition().OnEpsilon().From(subSequence.StartState).To(subSequence.EndState);
            }

            return subSequence.EndState;
        }
    }
}
