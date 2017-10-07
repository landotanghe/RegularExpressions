﻿using FiniteAutomota.NonDeterministic;
using FiniteAutomota.NonDeterministic.Builder;
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
            private StateNamer StateNamer;
            private PatternReader PatternReader;
            private AutomatonBuilder AutomatonBuilder;
            private int stateId = 0;

            public RegularExpressionBuilder(string pattern)
            {
                PatternReader = new PatternReader(pattern);
                AutomatonBuilder = new AutomatonBuilder();
                StateNamer = new StateNamer();
            }            

            public RegularExpression Build()
            {
                var sequence = ReadSequence();
                var automaton = AutomatonBuilder.Build();
                return new RegularExpression(automaton, sequence);
            }

            private Sequence ReadSequence()
            {
                var sequence = new Sequence
                {
                    StartState = StateNamer.CreateNameForStartOfSequence()
                };

                AutomatonBuilder.State(sequence.StartState).ActiveAtStart();

                var previousState = sequence.StartState;
                while (PatternReader.HasUnprocessedInput())
                {
                    var symbol = PatternReader.ReadNextSymbol();
                    var currentState = StateNamer.CreateNameForIntermediateState();

                    AutomatonBuilder.State(currentState)
                        .Transition().On(symbol).From(previousState).To(currentState);

                    previousState = currentState;
                }

                sequence.EndState = StateNamer.CreateNameForEndOfSequence();

                AutomatonBuilder.State(sequence.EndState)
                    .Transition().OnEpsilon().From(previousState).To(sequence.EndState);

                return sequence;
            }
        }

        internal class StateNamer
        {
            private const string StartStatePrefix = "Start ";
            private const string FinalStatePrefix = "Final ";
            private const string IntermediatePrefix = "Intermediate ";

            private int StateNumber = 0;

            public string CreateNameForStartOfSequence()
            {
                return StartStatePrefix + StateNumber++;
            }

            public string CreateNameForEndOfSequence()
            {
                return FinalStatePrefix + StateNumber++;
            }

            public string CreateNameForIntermediateState()
            {
                return IntermediatePrefix + StateNumber++;
            }
        }


        internal class Sequence
        {
            public string StartState { get; set; }
            public string EndState { get; set; }
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
