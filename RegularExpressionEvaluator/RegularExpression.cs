using FiniteAutomota.NonDeterministic;
using FiniteAutomota.NonDeterministic.Builder;
using System;
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
            public const char OrOperator = '|';
            public const char StartNewSequence = '(';
            public const char EndNewSequence = ')';
            public const char Tab = '\t';
            private readonly static char[] EscapableCharactersWithSpecialMeaning = { EscapeChar, '{', '}', StartNewSequence, EndNewSequence, OrOperator };

            private int nextSymbolPosition = 0;
            private string _input;
            public PatternReader(string input)
            {
                _input = input;
            }

            public Token ReadNextToken()
            {
                if (!HasUnprocessedInput())
                    return new Token(' ', TokenType.EndOfInput);

                var nextSymbol = _input[nextSymbolPosition++];
                if (nextSymbol == EscapeChar)
                {
                    return ReadEscapedToken();
                }
                else
                {
                    return CreateUnescapedToken(nextSymbol);
                }
            }


            private Token ReadEscapedToken()
            {
                if (!HasUnprocessedInput())
                    throw new System.Exception("expected input after \\");

                var nextSymbol = _input[nextSymbolPosition++];
                if (nextSymbol == 't')
                {
                    return new Token(Tab, TokenType.Character);
                }
                else if (EscapableCharactersWithSpecialMeaning.Contains(nextSymbol))
                {
                    return new Token(nextSymbol, TokenType.Character);
                }
                else
                {
                    throw new System.Exception($"unexpected input symbol {nextSymbol} at position {nextSymbolPosition-1}");
                }
            }

            private static Token CreateUnescapedToken(char nextSymbol)
            {
                if (nextSymbol == OrOperator)
                {
                    return new Token(OrOperator, TokenType.OrOperator);
                }
                if(nextSymbol == StartNewSequence)
                {
                    return new Token(StartNewSequence, TokenType.StartNewSequence);
                }
                if (nextSymbol == EndNewSequence)
                {
                    return new Token(EndNewSequence, TokenType.EndNewSequence);
                }
                return new Token(nextSymbol, TokenType.Character);
            }

            private bool HasUnprocessedInput()
            {
                return nextSymbolPosition < _input.Length;
            }
        }

        public enum TokenType
        {
            Character,
            OrOperator,
            EndOfInput,
            StartNewSequence,
            EndNewSequence
        }

        public class Token
        {
            public Token(char sybmol, TokenType tokenType)
            {
                Symbol = sybmol;
                TokenType = tokenType;
            }

            public char Symbol { get; }
            public TokenType TokenType { get; }
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
                    if(token.TokenType == TokenType.Character)
                    {
                        var currentState = StateNamer.CreateNameForIntermediateState();

                        AutomatonBuilder.State(currentState)
                            .Transition().On(token.Symbol).From(previousState).To(currentState);

                        previousState = currentState;
                    }else if(token.TokenType == TokenType.StartNewSequence)
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
            public Sequence(string startState, string endState)
            {
                StartState = startState;
                EndState = endState;
            }
            public string StartState { get;  }
            public string EndState { get;  }
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
